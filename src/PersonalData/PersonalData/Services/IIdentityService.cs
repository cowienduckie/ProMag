using Promag.Protobuf.Identity.V1;

namespace PersonalData.Services;

public interface IIdentityService
{
    Task<IReadOnlyDictionary<string, List<UserRoleDto>>> FetchUserRolesByPersonIds(
        IEnumerable<string> personIds,
        CancellationToken cancellationToken);

    Task<List<RoleDto>> FetchAllRoles(CancellationToken cancellationToken = default);

    Task<List<RoleDto>> FetchRoleById(IEnumerable<string> roleIds, CancellationToken cancellationToken = default);

    Task<List<RoleDto>> FetchRoleById(string roleId, CancellationToken cancellationToken = default);
}