using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Data;
using Shared.SecurityContext;

namespace PersonalData.UseCases.Commands.Handlers;

public class AcceptWorkspaceInvitationHandler : IRequestHandler<AcceptWorkspaceInvitationCommand, bool>
{
    private readonly ILogger<AcceptWorkspaceInvitationHandler> _logger;
    private readonly PersonalContext _personalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public AcceptWorkspaceInvitationHandler(ILogger<AcceptWorkspaceInvitationHandler> logger, PersonalContext personalContext,
        ISecurityContextAccessor securityContext)
    {
        _logger = logger;
        _personalContext = personalContext;
        _securityContext = securityContext;
    }

    public async Task<bool> Handle(AcceptWorkspaceInvitationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(AcceptWorkspaceInvitationHandler));

        var invitation = await _personalContext.WorkspaceInvitations
            .Include(i => i.Workspace)
            .ThenInclude(w => w.Members)
            .Include(i => i.InvitedPerson)
            .FirstOrDefaultAsync(i => i.WorkspaceId == Guid.Parse(request.WorkspaceId) && i.InvitedPersonId == Guid.Parse(_securityContext.UserId!),
                cancellationToken);

        if (invitation == null)
        {
            _logger.LogInformation("{HandlerName} - Invitation not found", nameof(AcceptWorkspaceInvitationHandler));

            return false;
        }

        invitation.Accepted = true;
        invitation.Workspace.Members.Add(invitation.InvitedPerson);

        _personalContext.WorkspaceInvitations.Update(invitation);
        await _personalContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{HandlerName} - Finish", nameof(AcceptWorkspaceInvitationHandler));

        return true;
    }
}