using MediatR;
using PersonalData.Data.Audit;

namespace PersonalData.UseCases.Commands;

[ActivityLog]
public class UpdateRolePermissionsCommand : IRequest<bool>
{
    public Guid RoleId { get; set; }

    public IList<string> Permissions { get; set; } = default!;
}