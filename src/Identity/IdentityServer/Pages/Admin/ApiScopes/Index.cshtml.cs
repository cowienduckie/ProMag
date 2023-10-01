using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Admin.ApiScopes;

[SecurityHeaders]
[Authorize]
public class IndexModel : PageModel
{
    private readonly ApiScopeRepository _repository;

    public IndexModel(ApiScopeRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ApiScopeSummaryModel> Scopes { get; private set; } = Enumerable.Empty<ApiScopeSummaryModel>();
    public string Filter { get; set; } = string.Empty;

    public async Task OnGetAsync(string filter)
    {
        Filter = filter;
        Scopes = await _repository.GetAllAsync(filter);
    }
}