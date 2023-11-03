namespace IdentityServer.Models.Consent;

public class ConsentViewModel : ConsentInputModel
{
    public string ClientName { get; set; } = default!;
    public string? ClientUrl { get; set; }
    public string? ClientLogoUrl { get; set; }
    public bool AllowRememberConsent { get; set; }

    public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();
    public IEnumerable<ScopeViewModel> ResourceScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();
}