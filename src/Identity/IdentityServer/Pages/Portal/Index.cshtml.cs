using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Portal;

public class Index : PageModel
{
    private readonly ClientRepository _repository;

    public Index(ClientRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ThirdPartyInitiatedLoginLink> Clients { get; private set; } = Array.Empty<ThirdPartyInitiatedLoginLink>();

    public async Task OnGetAsync()
    {
        Clients = await _repository.GetClientsWithLoginUris();
    }
}