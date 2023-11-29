using System.Diagnostics.CodeAnalysis;
using Configuration.MassTransit.IntegrationEvents.Account;
using Configuration.MassTransit.IntegrationEvents.Email;
using Configuration.MassTransit.IntegrationEvents.Logging;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using IdentityServer.Common.Attributes;
using IdentityServer.Common.Extensions;
using IdentityServer.Models.Account;
using IdentityServer.Models.DbModel;
using IdentityServer.Options;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Shared.Common.Enums;
using Shared.CustomTypes;

namespace IdentityServer.Boundaries.Controllers.Account;

[SecurityHeaders]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IBus _bus;
    private readonly IClientStore _clientStore;
    private readonly IConfiguration _configuration;
    private readonly IEventService _events;
    private readonly IAuthenticationHandlerProvider _handlerProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        IAuthenticationHandlerProvider handlerProvider,
        IEventService events,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        IBus bus)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _handlerProvider = handlerProvider;
        _events = events;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _bus = bus;
    }

    /// <summary>
    ///     Entry point into the login workflow
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        // build a model so we know what to show on the login page
        var viewModel = await BuildLoginViewModelAsync(returnUrl);

        if (viewModel.IsExternalLoginOnly)
        {
            // we only have one option for logging in and it's an external provider
            return RedirectToAction("Challenge", "External", new { provider = viewModel.ExternalLoginScheme, returnUrl });
        }

        return View(viewModel);
    }

    /// <summary>
    ///     Handle postback from username/password login
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [SuppressMessage("ReSharper", "CognitiveComplexity")]
    public async Task<IActionResult> Login(LoginInputModel model, string button)
    {
        var startTime = DateTime.Now;

        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        // the user clicked the "cancel" button
        if (button != "login")
        {
            // since we don't have a valid context, then we just go back to the home page
            if (context == null)
            {
                return Redirect("~/");
            }

            // if the user cancels, send a result back into IdentityServer as if they
            // denied the consent (even if this client does not require consent).
            // this will send back an access denied OIDC error response to the client.
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            if (await _clientStore.IsPkceClientAsync(context.Client.ClientId))
            {
                // if the client is PKCE then we assume it's native, so this change in how to
                // return the response is for better UX for the end user.
                return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
            }

            return Redirect(model.ReturnUrl);
        }

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username.ToLower(), model.Password, model.RememberLogin, true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                await _events.RaiseAsync(new UserLoginSuccessEvent(user!.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                // Save activity log when login succeeded.
                var duration = DateTime.Now.Subtract(startTime);
                model.Password = string.Empty;
                await SendActivityLogEvent(nameof(UserLoginSuccessEvent), duration.Milliseconds, model, user.UserName ?? string.Empty);

                if (context != null)
                {
                    if (await _clientStore.IsPkceClientAsync(context.Client.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(model.ReturnUrl);
                }

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect("~/");
                }

                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, AccountOptions.LockedOutErrorMessage);
                var user = await _userManager.FindByNameAsync(model.Username);

                await _bus.Send(new AccountStatusChanged(
                    Guid.NewGuid(),
                    user!.Id,
                    UserStatus.Lock
                ));
            }
            else
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }
        }

        // something went wrong, show form with error
        var viewModel = await BuildLoginViewModelAsync(model);

        return View(viewModel);
    }

    /// <summary>
    ///     Show logout page
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToAction("Login");
        }

        // build a model so the logout page knows what to display
        var viewModel = await BuildLogoutViewModelAsync(logoutId);

        return await Logout(viewModel);
    }

    /// <summary>
    ///     Handle logout page postback
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        var startTime = DateTime.Now;
        // build a model so the logged out page knows what to display
        var viewModel = await BuildLoggedOutViewModelAsync(model.LogoutId);

        if (User.Identity?.IsAuthenticated == true)
        {
            // delete local authentication cookie
            await _signInManager.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        // Save activity log when LoggedOut.
        var duration = DateTime.Now.Subtract(startTime);
        await SendActivityLogEvent(nameof(UserLogoutSuccessEvent), duration.Milliseconds, model, User.Identity?.Name ?? string.Empty);

        // check if we need to trigger sign-out at an upstream identity provider
        if (!viewModel.TriggerExternalSignout)
        {
            return RedirectToAction("LoggedOut", viewModel);
        }

        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        var url = Url.Action("Logout", new { logoutId = viewModel.LogoutId });

        // this triggers a redirect to the external provider for sign-out
        return SignOut(new AuthenticationProperties { RedirectUri = url }, viewModel.ExternalAuthenticationScheme ?? string.Empty);
    }

    [HttpGet]
    public IActionResult LoggedOut(LoggedOutViewModel viewModel)
    {
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("", "Your Email is required");

            return View();
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            ModelState.AddModelError("", "Email not found");

            return View();
        }

        var tokenGenerated = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetLink = Url.Action("ResetPassword", "Account", new
        {
            userId = user.Id, token = tokenGenerated
        }, Request.Scheme);

        await _bus.Send(new SendResetPasswordEmail
        (
            Guid.NewGuid(),
            resetLink ?? string.Empty,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty
        ));

        return View("EmailSent");
    }

    [HttpGet]
    public IActionResult AlertPasswordUpdated()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            ModelState.AddModelError("", "User is not found");

            return View("UpdatePassword");
        }

        return View("UpdatePassword", new InputPasswordViewModel
        {
            UserId = userId,
            Token = token,
            UserName = user.UserName,
            TokenPurpose = "ResetPasswordTokenPurpose"
        });
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string id, string token)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            ModelState.AddModelError("", "User is not found");

            return View("UpdatePassword");
        }

        var confirmUserResult = await _userManager.ConfirmEmailAsync(user, token);

        if (confirmUserResult.Succeeded)
        {
            return View("UpdatePassword", new InputPasswordViewModel
            {
                UserId = id,
                Token = token,
                UserName = user.UserName,
                TokenPurpose = "ConfirmEmailTokenPurpose"
            });
        }

        ModelState.AddModelError("", "Your activation code is not valid");

        return View("UpdatePassword");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword(InputPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return View(model);
        }

        var result = model.TokenPurpose switch
        {
            "ResetPasswordTokenPurpose" => await _userManager.ResetPasswordAsync(user, model.Token, model.Password),
            "ConfirmEmailTokenPurpose" => await _userManager.AddPasswordAsync(user, model.Password),
            _ => null
        };

        if (result is { Succeeded: false })
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return View(model);
        }

        await _bus.Send(new AccountStatusChanged(Guid.NewGuid(), user.Id, UserStatus.Active));

        return View(nameof(AlertPasswordUpdated));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    #region API Helpers

    private async Task<LoginViewModel> BuildLoginViewModelAsync(string? returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var viewModel = new LoginViewModel
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl ?? "~/",
                Username = context.LoginHint ?? string.Empty
            };

            if (!local)
            {
                viewModel.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
            }

            return viewModel;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();
        var providers = schemes
            .Where(x => x.DisplayName != null ||
                        x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase)
            )
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName,
                AuthenticationScheme = x.Name
            }).ToList();

        var allowLocal = true;

        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;

                if (client.IdentityProviderRestrictions.Any())
                {
                    providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }
        }

        return new LoginViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl ?? "~/",
            Username = context?.LoginHint ?? string.Empty,
            ExternalProviders = providers.ToArray()
        };
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
    {
        var viewModel = await BuildLoginViewModelAsync(model.ReturnUrl);

        viewModel.Username = model.Username;
        viewModel.RememberLogin = model.RememberLogin;

        return viewModel;
    }

    private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
    {
        var viewModel = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        if (User.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            viewModel.ShowLogoutPrompt = false;
            return viewModel;
        }

        var context = await _interaction.GetLogoutContextAsync(logoutId);
        if (context.ShowSignoutPrompt)
        {
            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return viewModel;
        }

        // it's safe to automatically sign-out
        viewModel.ShowLogoutPrompt = false;

        return viewModel;
    }

    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        var viewModel = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout.ClientName) ? logout.ClientId : logout.ClientName,
            SignOutIframeUrl = logout.SignOutIFrameUrl,
            LogoutId = logoutId
        };

        if (User.Identity?.IsAuthenticated != true)
        {
            return viewModel;
        }

        var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

        if (idp is null or IdentityServerConstants.LocalIdentityProvider)
        {
            return viewModel;
        }

        var handler = await _handlerProvider.GetHandlerAsync(HttpContext, idp);

        if (handler is not IAuthenticationSignOutHandler)
        {
            return viewModel;
        }

        // if there's no current logout context, we need to create one
        // this captures necessary info from the current logged in user
        // before we signout and redirect away to the external IdP for signout
        viewModel.LogoutId ??= await _interaction.CreateLogoutContextAsync();
        viewModel.ExternalAuthenticationScheme = idp;

        return viewModel;
    }

    private async Task SendActivityLogEvent(string action, long duration, object parameter, string username)
    {
        var appOptions = _configuration.GetOptions<AppOptions>("App");

        await _bus.Send<SaveActivityLog>(new
        {
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
            Service = appOptions.Name,
            Action = action,
            Duration = duration,
            Parameters = JsonConvert.SerializeObject(parameter),
            Username = username
        });
    }

    #endregion
}