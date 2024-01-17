using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Common.Converters;
using PersonalData.Data;
using Shared.SecurityContext;

namespace PersonalData.UseCases.Queries.Handlers;

public class GetWorkspaceByIdHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto?>
{
    private readonly ILogger<GetWorkspaceByIdHandler> _logger;
    private readonly PersonalContext _personalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public GetWorkspaceByIdHandler(ILogger<GetWorkspaceByIdHandler> logger, PersonalContext personalContext, ISecurityContextAccessor securityContext)
    {
        _logger = logger;
        _personalContext = personalContext;
        _securityContext = securityContext;
    }

    public async Task<WorkspaceDto?> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetWorkspaceByIdHandler));

        if (!Guid.TryParse(_securityContext.UserId, out var userId))
        {
            _logger.LogError("{HandlerName} - UserId is not a valid Guid", nameof(GetWorkspaceByIdHandler));

            return null;
        }

        var workspace = await _personalContext.Workspaces
            .Include(w => w.Members)
            .Include(w => w.Invitations)
            .ThenInclude(w => w.InvitedPerson)
            .SingleOrDefaultAsync(w => w.Id == Guid.Parse(request.WorkspaceId) && w.DeletedOn == null, cancellationToken);

        if (workspace == null)
        {
            _logger.LogError("{HandlerName} - Workspace not found", nameof(GetWorkspaceByIdHandler));

            return null;
        }

        if (workspace.Members.All(m => m.Id != userId))
        {
            _logger.LogError("{HandlerName} - User is not a member of the workspace", nameof(GetWorkspaceByIdHandler));

            return null;
        }

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetWorkspaceByIdHandler));

        return workspace.ToWorkspaceDto();
    }
}