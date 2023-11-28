using Microsoft.Extensions.Logging;
using Promag.Protobuf.Identity.V1;

namespace PersonalData.Services.Implementations;

public class IdentityService : IIdentityService
{
    private readonly IdentityApi.IdentityApiClient _apiClient;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(IdentityApi.IdentityApiClient apiClient, ILogger<IdentityService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<IReadOnlyDictionary<string, List<UserRoleDto>>> FetchUserRolesByPersonIds(
        IEnumerable<string> personIds,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Service} - {Method} - Start", nameof(IdentityService), nameof(FetchUserRolesByPersonIds));

        var request = new GetUserRolesByUserIdsRequest();
        request.UserIds.AddRange(personIds);

        var result = await _apiClient.GetUserRolesByUserIdsAsync(request, cancellationToken: cancellationToken);

        var userRoles = result.UserRoles
            .GroupBy(x => x.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                Roles = g.Select(r => r).ToList()
            })
            .ToDictionary(k => k.UserId, kv => kv.Roles);

        _logger.LogInformation("{Service} - {Method} - Finish", nameof(IdentityService), nameof(FetchUserRolesByPersonIds));

        return userRoles;
    }

    public async Task<List<RoleDto>> FetchAllRoles(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("{Service} - {Method} - Start", nameof(IdentityService), nameof(FetchAllRoles));

        var response = await _apiClient.GetRolesAsync(new GetRoleByIdsRequest(), cancellationToken: cancellationToken);

        _logger.LogInformation("{Service} - {Method} - Finish", nameof(IdentityService), nameof(FetchAllRoles));

        return response.Roles.ToList();
    }

    public async Task<List<RoleDto>> FetchRoleById(IEnumerable<string> roleIds, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("{Service} - {Method} - Start", nameof(IdentityService), nameof(FetchRoleById));

        var request = new GetRoleByIdsRequest();
        request.RoleIds.AddRange(roleIds);

        var response = await _apiClient.GetRolesAsync(request, cancellationToken: cancellationToken);

        _logger.LogInformation("{Service} - {Method} - Finish", nameof(IdentityService), nameof(FetchRoleById));

        return response.Roles.ToList();
    }

    public async Task<List<RoleDto>> FetchRoleById(string roleId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("{Service} - {Method} - Start", nameof(IdentityService), nameof(FetchRoleById));

        var request = new GetRoleByIdsRequest();
        request.RoleIds.Add(roleId);

        var response = await _apiClient.GetRolesAsync(request, cancellationToken: cancellationToken);

        _logger.LogInformation("{Service} - {Method} - Finish", nameof(IdentityService), nameof(FetchRoleById));

        return response.Roles.ToList();
    }
}