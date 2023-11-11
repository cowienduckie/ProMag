namespace IdentityServer.Models.Consent;

public class ConsentInputModel
{
    public string Button { get; set; } = default!;
    public IEnumerable<string> ScopesConsented { get; set; } = new List<string>();
    public bool RememberConsent { get; set; } = true;
    public string ReturnUrl { get; set; } = default!;
}