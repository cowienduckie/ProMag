using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityServer.Common.Attributes;
using IdentityServer.Common.Extensions;
using IdentityServer.Models.Account;
using IdentityServer.Models.Consent;
using IdentityServer.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Boundaries.Controllers.Consent;

/// <summary>
///     This controller processes the consent UI
/// </summary>
[SecurityHeaders]
[Authorize]
public class ConsentController : Controller
{
    private readonly IClientStore _clientStore;
    private readonly IEventService _events;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<ConsentController> _logger;
    private readonly IResourceStore _resourceStore;

    public ConsentController(
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IResourceStore resourceStore,
        IEventService events,
        ILogger<ConsentController> logger)
    {
        _interaction = interaction;
        _clientStore = clientStore;
        _resourceStore = resourceStore;
        _events = events;
        _logger = logger;
    }

    /// <summary>
    ///     Shows the consent screen
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Index(string returnUrl)
    {
        var viewModel = await BuildViewModelAsync(returnUrl);

        if (viewModel != null)
        {
            return View("Index", viewModel);
        }

        return View("Error");
    }

    /// <summary>
    ///     Handles the consent screen postback
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ConsentInputModel model)
    {
        var result = await ProcessConsent(model);

        if (result.IsRedirect)
        {
            if (await _clientStore.IsPkceClientAsync(result.ClientId))
            {
                // if the client is PKCE then we assume it's native, so this change in how to
                // return the response is for better UX for the end user.
                return View("Redirect", new RedirectViewModel { RedirectUrl = result.RedirectUri! });
            }

            return Redirect(result.RedirectUri!);
        }

        if (result.HasValidationError)
        {
            ModelState.AddModelError(string.Empty, result.ValidationError!);
        }

        if (result.ShowView)
        {
            return View("Index", result.ViewModel);
        }

        return View("Error");
    }

    #region API Helpers

    private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
    {
        var result = new ProcessConsentResult();

        // validate return url is still valid
        var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
        if (request == null)
        {
            return result;
        }

        ConsentResponse? grantedConsent = null;

        switch (model.Button)
        {
            case "no": // user clicked 'no' - send back the standard 'access_denied' response
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

                await _events.RaiseAsync(new ConsentDeniedEvent(
                    User.GetSubjectId(),
                    request.Client.ClientId,
                    request.ValidatedResources.RawScopeValues));

                break;
            case "yes": // user clicked 'yes' - validate the data
            {
                var scopes = model.ScopesConsented;

                if (ConsentOptions.EnableOfflineAccess == false)
                {
                    scopes = scopes.Where(x => x != IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                grantedConsent = new ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray()
                };

                await _events.RaiseAsync(new ConsentGrantedEvent(
                    User.GetSubjectId(),
                    request.Client.ClientId,
                    request.ValidatedResources.RawScopeValues,
                    grantedConsent.ScopesValuesConsented,
                    grantedConsent.RememberConsent));

                break;
            }
            default:
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
                break;
        }

        if (grantedConsent != null)
        {
            // communicate outcome of consent back to identity server
            await _interaction.GrantConsentAsync(request, grantedConsent);

            // indicate that's it ok to redirect back to authorization endpoint
            result.RedirectUri = model.ReturnUrl;
            result.ClientId = request.Client.ClientId;
        }
        else
        {
            // we need to redisplay the consent UI
            result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
        }

        return result;
    }

    private async Task<ConsentViewModel?> BuildViewModelAsync(string returnUrl, ConsentInputModel? model = null)
    {
        var request = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (request is null)
        {
            _logger.LogError("No consent request matching request: {ReturnUrl}", returnUrl);

            return null;
        }

        var client = await _clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);

        if (client is null)
        {
            _logger.LogError("Invalid client id: {ClientId}", request.Client.ClientId);

            return null;
        }

        var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ValidatedResources.RawScopeValues);

        if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
        {
            return CreateConsentViewModel(model, returnUrl, client, resources);
        }

        _logger.LogError("No scopes matching: {ScopesRequested}", request.ValidatedResources.RawScopeValues.Aggregate((x, y) => x + ", " + y));

        return null;
    }

    private static ConsentViewModel CreateConsentViewModel(ConsentInputModel? model, string returnUrl, Client client, Resources resources)
    {
        var viewModel = new ConsentViewModel
        {
            RememberConsent = model?.RememberConsent ?? true,
            ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

            ReturnUrl = returnUrl,

            ClientName = client.ClientName ?? client.ClientId,
            ClientUrl = client.ClientUri,
            ClientLogoUrl = client.LogoUri,
            AllowRememberConsent = client.AllowRememberConsent
        };

        viewModel.IdentityScopes = resources.IdentityResources
            .Select(x => CreateScopeViewModel(x, viewModel.ScopesConsented.Contains(x.Name) || model == null))
            .ToArray();

        viewModel.ResourceScopes = resources.ApiScopes
            .Select(x => CreateScopeViewModel(x, viewModel.ScopesConsented.Contains(x.Name) || model == null))
            .ToArray();

        if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
        {
            viewModel.ResourceScopes = viewModel.ResourceScopes.Union(new[]
            {
                GetOfflineAccessScope(viewModel.ScopesConsented.Contains(IdentityServerConstants.StandardScopes.OfflineAccess) ||
                                      model == null)
            });
        }

        return viewModel;
    }

    private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ScopeViewModel
        {
            Name = identity.Name,
            DisplayName = identity.DisplayName ?? string.Empty,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    private static ScopeViewModel CreateScopeViewModel(ApiScope scope, bool check)
    {
        return new ScopeViewModel
        {
            Name = scope.Name,
            DisplayName = scope.DisplayName ?? string.Empty,
            Description = scope.Description,
            Emphasize = scope.Emphasize,
            Required = scope.Required,
            Checked = check || scope.Required
        };
    }

    private static ScopeViewModel GetOfflineAccessScope(bool check)
    {
        return new ScopeViewModel
        {
            Name = IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = ConsentOptions.OfflineAccessDisplayName,
            Description = ConsentOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }

    #endregion
}