using MediatR;

namespace PersonalData.UseCases.Queries;

public class GetRolePermissionsQuery : IRequest<IEnumerable<string>>
{
    public GetRolePermissionsQuery(Guid roleId)
    {
        RoleId = roleId;
    }

    public Guid RoleId { get; set; }
}