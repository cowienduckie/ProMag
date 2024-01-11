using MediatR;

namespace PersonalData.UseCases.Commands;

public class AcceptWorkspaceInvitationCommand : IRequest<bool>
{
    public string WorkspaceId { get; set; } = default!;
}