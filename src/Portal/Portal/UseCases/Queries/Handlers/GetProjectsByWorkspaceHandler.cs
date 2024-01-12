using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Boundaries.GraphQL.Dtos.Projects;
using Portal.Data;
using Portal.Services;
using Shared.SecurityContext;

namespace Portal.UseCases.Queries.Handlers;

public class GetProjectsByWorkspaceHandler : IRequestHandler<GetProjectsByWorkspaceQuery, IEnumerable<SimplifiedProjectDto>>
{
    private readonly IAccessPermissionService _accessPermissionService;
    private readonly ILogger<GetProjectsByWorkspaceHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public GetProjectsByWorkspaceHandler(
        ILogger<GetProjectsByWorkspaceHandler> logger,
        PortalContext portalContext,
        ISecurityContextAccessor securityContext,
        IAccessPermissionService accessPermissionService)
    {
        _logger = logger;
        _portalContext = portalContext;
        _securityContext = securityContext;
        _accessPermissionService = accessPermissionService;
    }

    public async Task<IEnumerable<SimplifiedProjectDto>> Handle(GetProjectsByWorkspaceQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetProjectsByWorkspaceHandler));

        if (!Guid.TryParse(_securityContext.UserId, out var userId))
        {
            _logger.LogError("{HandlerName} - UserId is not a valid Guid", nameof(GetProjectsByWorkspaceHandler));

            return Enumerable.Empty<SimplifiedProjectDto>().AsQueryable();
        }

        var (workspaces, _) = await _accessPermissionService.GetUserCollaboration(userId);

        var projectDtos = _portalContext.Projects
            .Where(p => p.DeletedOn == null
                        && workspaces.Contains(p.WorkspaceId)
                        && p.WorkspaceId == Guid.Parse(request.WorkspaceId))
            .OrderByDescending(p => p.LastModifiedOn)
            .ThenByDescending(p => p.CreatedOn)
            .Select(p => p.ToSimplifiedProjectDto())
            .ToList();

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetProjectsByWorkspaceHandler));

        return projectDtos;
    }
}