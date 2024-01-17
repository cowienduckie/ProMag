using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Data;
using PersonalData.Domain;

namespace PersonalData.UseCases.Commands.Handlers;

public class InviteUserToWorkspaceHandler : IRequestHandler<InviteUserToWorkspaceCommand, bool>
{
    private readonly ILogger<InviteUserToWorkspaceHandler> _logger;
    private readonly PersonalContext _personalContext;

    public InviteUserToWorkspaceHandler(ILogger<InviteUserToWorkspaceHandler> logger, PersonalContext personalContext)
    {
        _logger = logger;
        _personalContext = personalContext;
    }

    public async Task<bool> Handle(InviteUserToWorkspaceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(InviteUserToWorkspaceHandler));

        var workspace = await _personalContext.Workspaces
            .Include(w => w.Members)
            .Include(w => w.Invitations)
            .FirstOrDefaultAsync(w => w.Id == request.WorkspaceId, cancellationToken);

        if (workspace == null)
        {
            _logger.LogInformation("{HandlerName} - Workspace not found", nameof(InviteUserToWorkspaceHandler));

            return false;
        }

        var invitedUser = await _personalContext.People.FirstOrDefaultAsync(p => p.Email.ToLower() == request.Email.ToLower(), cancellationToken);

        if (invitedUser == null)
        {
            _logger.LogInformation("{HandlerName} - User not found", nameof(InviteUserToWorkspaceHandler));

            return false;
        }

        if (workspace.Members.Any(m => m.Id == invitedUser.Id))
        {
            _logger.LogInformation("{HandlerName} - User is already a member of the workspace", nameof(InviteUserToWorkspaceHandler));

            return false;
        }

        if (workspace.Invitations.Any(i => i.InvitedPersonId == invitedUser.Id))
        {
            _logger.LogInformation("{HandlerName} - User is already invited to the workspace", nameof(InviteUserToWorkspaceHandler));

            return false;
        }

        var invitation = new WorkspaceInvitation
        {
            InvitedPersonId = invitedUser.Id,
            WorkspaceId = workspace.Id,
            Accepted = false
        };

        await _personalContext.WorkspaceInvitations.AddAsync(invitation, cancellationToken);
        await _personalContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{HandlerName} - End", nameof(InviteUserToWorkspaceHandler));

        return true;
    }
}