namespace IdentityServer.Models.Consent;

public class ConsentInputModel
{
    public string Button { get; set; } = default!;
    public IEnumerable<string> ScopesConsented { get; set; } = Enumerable.Empty<string>();
    public bool RememberConsent { get; set; }
    public string ReturnUrl { get; set; } = default!;
}