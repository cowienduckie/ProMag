using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Admin.Clients;

[SecurityHeaders]
[Authorize]
public class IndexModel : PageModel
{
    private readonly ClientRepository _repository;

    public IndexModel(ClientRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ClientSummaryModel> Clients { get; private set; } = Enumerable.Empty<ClientSummaryModel>();
    public string Filter { get; set; } = default!;

    public async Task OnGetAsync(string filter)
    {
        Filter = filter;
        Clients = await _repository.GetAllAsync(filter);
    }
}