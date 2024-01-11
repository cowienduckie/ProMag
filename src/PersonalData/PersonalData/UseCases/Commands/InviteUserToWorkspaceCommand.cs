using MediatR;

namespace PersonalData.UseCases.Commands;

public class InviteUserToWorkspaceCommand : IRequest<bool>
{
    public Guid WorkspaceId { get; set; }
    public string Email { get; set; } = string.Empty;
}