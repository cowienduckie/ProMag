using MediatR;

namespace PersonalData.UseCases.Commands;

public class DeleteWorkspaceCommand : IRequest<bool>
{
    public Guid WorkspaceId { get; set; }
}