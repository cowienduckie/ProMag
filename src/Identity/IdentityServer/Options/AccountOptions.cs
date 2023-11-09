using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace IdentityServer.Options;

[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
[SuppressMessage("ReSharper", "ConvertToConstant.Global")]
[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")]
public static class AccountOptions
{
    public static bool AllowLocalLogin = true;
    public static bool AllowRememberLogin = true;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    public static bool ShowLogoutPrompt = true;
    public static bool AutomaticRedirectAfterSignOut = true;

    // specify the Windows authentication scheme being used
    public static readonly string WindowsAuthenticationSchemeName = IISDefaults.AuthenticationScheme;

    // if user uses windows auth, should we load the groups from windows
    public static bool IncludeWindowsGroups = false;

    public static string InvalidCredentialsErrorMessage = "Invalid username or password";

    public static string LockedOutErrorMessage = "Your account has been locked out. Please contact your system administrator for details.";
}