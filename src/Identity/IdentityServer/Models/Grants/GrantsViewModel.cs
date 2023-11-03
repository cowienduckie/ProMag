namespace IdentityServer.Models.Grants;

public class GrantsViewModel
{
    public IEnumerable<GrantViewModel> Grants { get; set; } = Enumerable.Empty<GrantViewModel>();
}

public class GrantViewModel
{
    public string ClientId { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string? ClientUrl { get; set; }
    public string? ClientLogoUrl { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Expires { get; set; }
    public IEnumerable<string> IdentityGrantNames { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<string> ApiGrantNames { get; set; } = Enumerable.Empty<string>();
}