using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Common.Converters;
using PersonalData.Data;
using PersonalData.UseCases.Responses;
using Shared.SecurityContext;

namespace PersonalData.UseCases.Queries.Handlers;

public class GetWorkspacesHandler : IRequestHandler<GetWorkspacesQuery, GetWorkspacesResponse>
{
    private readonly ILogger<GetWorkspacesHandler> _logger;
    private readonly PersonalContext _personalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public GetWorkspacesHandler(ILogger<GetWorkspacesHandler> logger, PersonalContext personalContext, ISecurityContextAccessor securityContext)
    {
        _logger = logger;
        _personalContext = personalContext;
        _securityContext = securityContext;
    }

    public async Task<GetWorkspacesResponse> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetWorkspacesHandler));

        if (!Guid.TryParse(_securityContext.UserId, out var userId))
        {
            _logger.LogError("{HandlerName} - UserId is not a valid Guid", nameof(GetWorkspacesHandler));

            return new GetWorkspacesResponse();
        }

        var workspaceDtos = await _personalContext.Workspaces
            .Include(w => w.Members)
            .Include(w => w.Invitations)
            .ThenInclude(w => w.InvitedPerson)
            .Where(w => w.DeletedOn == null
                        && (w.CreatedBy == userId || w.Members.Any(m => m.Id == userId) || w.Invitations.Any(i => i.InvitedPersonId == userId))
            )
            .OrderBy(w => w.Name)
            .Select(w => w.ToWorkspaceDto())
            .ToListAsync(cancellationToken);

        var ownedWorkspaces = workspaceDtos.Where(w => w.OwnerId == userId.ToString()).ToList();
        var memberWorkspaces = workspaceDtos.Where(w => w.OwnerId != userId.ToString() && w.Members.Any(m => m.Id == userId.ToString())).ToList();
        var pendingWorkspaces = workspaceDtos.Where(w => w.Invitations.Any(i => i.Id == userId.ToString())).ToList();

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetWorkspacesHandler));

        return new GetWorkspacesResponse
        {
            OwnedWorkspaces = ownedWorkspaces,
            MemberWorkspaces = memberWorkspaces,
            PendingWorkspaces = pendingWorkspaces
        };
    }
}