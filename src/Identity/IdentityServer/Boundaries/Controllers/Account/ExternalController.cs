using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using IdentityServer.Common.Attributes;
using IdentityServer.Common.Extensions;
using IdentityServer.Models.Account;
using IdentityServer.Models.DbModel;
using IdentityServer.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Boundaries.Controllers.Account;

[SecurityHeaders]
[AllowAnonymous]
public class ExternalController : Controller
{
    private readonly IClientStore _clientStore;
    private readonly IEventService _events;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<ExternalController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExternalController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IEventService events,
        ILogger<ExternalController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _clientStore = clientStore;
        _events = events;
        _logger = logger;
    }

    /// <summary>
    ///     Initiate roundtrip to external authentication provider
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Challenge(string provider, string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = "~/";
        }

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
        {
            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        }

        if (AccountOptions.WindowsAuthenticationSchemeName == provider)
        {
            // windows authentication needs special handling
            return await ProcessWindowsLoginAsync(returnUrl);
        }

        // start challenge and roundtrip the return URL and scheme
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(Callback)),
            Items =
            {
                { "returnUrl", returnUrl },
                { "scheme", provider }
            }
        };

        return Challenge(props, provider);
    }

    /// <summary>
    ///     Post processing of external authentication
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Callback()
    {
        // read external identity from the temporary cookie
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

        if (result.Succeeded != true)
        {
            throw new Exception("External authentication error");
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = result.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");

            _logger.LogDebug("External claims: {Claims}", externalClaims);
        }

        // lookup our user and external provider info
        var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);

        // this might be where you might initiate a custom workflow for user registration
        // in this sample we don't show how that would be done, as our sample implementation
        // simply auto-provisions new external user
        user ??= await AutoProvisionUserAsync(provider, providerUserId, claims);

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);
        // ProcessLoginCallbackForWsFed(result, additionalLocalClaims, localSignInProps);
        // ProcessLoginCallbackForSaml2P(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        // we must issue the cookie manually, and can't use the SignInManager because
        // it doesn't expose an API to issue additional claims from the login workflow
        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        additionalLocalClaims.AddRange(principal.Claims);

        var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id;

        await HttpContext.SignInAsync(provider, principal, localSignInProps);

        // delete temporary cookie used during external authentication
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // retrieve return URL
        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        // check if external login is in the context of an OIDC request
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, name, true, context?.Client.ClientId));

        if (context != null && await _clientStore.IsPkceClientAsync(context.Client.ClientId))
        {
            // if the client is PKCE then we assume it's native, so this change in how to
            // return the response is for better UX for the end user.
            return View("Redirect", new RedirectViewModel { RedirectUrl = returnUrl });
        }

        return Redirect(returnUrl);
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
    {
        // see if windows auth has already been requested and succeeded
        var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);

        if (result.Principal is not WindowsPrincipal windowsPrincipal)
        {
            // trigger windows auth
            // since windows auth don't support the redirect uri,
            // this URL is re-triggered when we call challenge
            return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
        }

        // we will issue the external cookie and then redirect the
        // user back to the external callback, in essence, treating windows
        // auth the same as any other external authentication mechanism
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("Callback"),
            Items =
            {
                { "returnUrl", returnUrl },
                { "scheme", AccountOptions.WindowsAuthenticationSchemeName }
            }
        };

        var identity = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
        identity.AddClaim(new Claim(JwtClaimTypes.Subject, windowsPrincipal.Identity.Name!));
        identity.AddClaim(new Claim(JwtClaimTypes.Name, windowsPrincipal.Identity.Name!));

        // add the groups as claims -- be careful if the number of groups is too large
        if (AccountOptions.IncludeWindowsGroups)
        {
            var windowsIdentity = windowsPrincipal.Identity as WindowsIdentity;
            var groups = windowsIdentity!.Groups!.Translate(typeof(NTAccount));
            var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
            identity.AddClaims(roles);
        }

        await HttpContext.SignInAsync(
            IdentityServerConstants.ExternalCookieAuthenticationScheme,
            new ClaimsPrincipal(identity),
            props);

        return Redirect(props.RedirectUri ?? "~/");
    }

    private async Task<(ApplicationUser? user, string provider, string providerUserId, IEnumerable<Claim> claims)>
        FindUserFromExternalProviderAsync(AuthenticateResult result)
    {
        var externalUser = result.Principal;

        // try to determine the unique id of the external user (issued by the provider)
        // the most common claim type for that are the sub claim and the NameIdentifier
        // depending on the external provider, some other claim type might be used
        var userIdClaim = externalUser?.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser?.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new Exception("Unknown userid");

        // remove the user id claim so we don't include it as an extra claim if/when we provision the user
        var claims = externalUser.Claims.ToList();
        claims.Remove(userIdClaim);

        var provider = result.Properties!.Items["scheme"];
        var providerUserId = userIdClaim.Value;

        // find external user
        var user = await _userManager.FindByLoginAsync(provider!, providerUserId);

        return (user, provider!, providerUserId, claims);
    }

    private async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
    {
        // Create a list of claims that we want to transfer into our store
        var filtered = new List<Claim>();

        // User's display name
        var claimList = claims.ToList();

        var name = claimList.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                   claimList.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (name != null)
        {
            filtered.Add(new Claim(JwtClaimTypes.Name, name));
        }
        else
        {
            var first = claimList.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                        claimList.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var last = claimList.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                       claimList.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            if (first != null && last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
            }
            else if (first != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first));
            }
            else if (last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, last));
            }
        }

        // Email
        var email = claimList.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                    claimList.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (email != null)
        {
            filtered.Add(new Claim(JwtClaimTypes.Email, email));
        }

        var user = new ApplicationUser
        {
            UserName = Guid.NewGuid().ToString()
        };
        var identityResult = await _userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors.First().Description);
        }

        if (filtered.Any())
        {
            identityResult = await _userManager.AddClaimsAsync(user, filtered);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }
        }

        identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors.First().Description);
        }

        return user;
    }


    private static void ProcessLoginCallbackForOidc(
        AuthenticateResult externalResult,
        ICollection<Claim> localClaims,
        AuthenticationProperties localSignInProps)
    {
        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult.Principal?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        // if the external provider issued an id_token, we'll keep it for signout
        var idToken = externalResult.Properties?.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
        }
    }

    // private static void ProcessLoginCallbackForWsFed(
    //     AuthenticateResult externalResult,
    //     List<Claim> localClaims,
    //     AuthenticationProperties localSignInProps)
    // {
    // }
    //
    // private static void ProcessLoginCallbackForSaml2P(
    //     AuthenticateResult externalResult,
    //     List<Claim> localClaims,
    //     AuthenticationProperties localSignInProps)
    // {
    // }
}