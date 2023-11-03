using Microsoft.AspNetCore.Server.IISIntegration;

namespace IdentityServer.Options;

public static class AccountOptions
{
    public static bool AllowLocalLogin = true;
    public static bool AllowRememberLogin = false;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    public static bool ShowLogoutPrompt = true;
    public static bool AutomaticRedirectAfterSignOut = false;

    // specify the Windows authentication scheme being used
    public static readonly string WindowsAuthenticationSchemeName = IISDefaults.AuthenticationScheme;

    // if user uses windows auth, should we load the groups from windows
    public static bool IncludeWindowsGroups = false;

    public static string InvalidCredentialsErrorMessage = "Invalid username or password";

    public static string LockedOutErrorMessage = "Your account has been locked out. Please contact your system administrator for details.";
}