using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Boundaries.GraphQL.Dtos.Projects;
using Portal.Data;
using Portal.Services;
using Shared.SecurityContext;

namespace Portal.UseCases.Queries.Handlers;

public class GetKanbanProjectByIdHandler : IRequestHandler<GetKanbanProjectByIdQuery, KanbanProjectDto?>
{
    private readonly IAccessPermissionService _accessPermissionService;
    private readonly ILogger<GetKanbanProjectByIdHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public GetKanbanProjectByIdHandler(
        ILogger<GetKanbanProjectByIdHandler> logger,
        PortalContext portalContext,
        ISecurityContextAccessor securityContext,
        IAccessPermissionService accessPermissionService)
    {
        _logger = logger;
        _portalContext = portalContext;
        _securityContext = securityContext;
        _accessPermissionService = accessPermissionService;
    }

    public async Task<KanbanProjectDto?> Handle(GetKanbanProjectByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start - Project ID={ProjectId}", nameof(GetKanbanProjectByIdHandler), request.ProjectId);

        if (!Guid.TryParse(request.ProjectId, out var projectId))
        {
            _logger.LogError("{Handler} - Invalid Project ID", nameof(GetKanbanProjectByIdHandler));

            return null;
        }

        try
        {
            var project = await _portalContext.Projects
                .Where(p => p.Id == projectId
                            && p.DeletedOn == null)
                .Include(p => p.Tasks
                    .Where(t => t.DeletedOn == null))
                .Include(p => p.Sections)
                .ThenInclude(s => s.Tasks
                    .Where(t => t.DeletedOn == null))
                .SingleOrDefaultAsync(cancellationToken);

            if (project is null)
            {
                _logger.LogError("{Handler} - Project not found", nameof(GetKanbanProjectByIdHandler));

                return null;
            }

            if (await _accessPermissionService.HasAccessToProject(Guid.Parse(_securityContext.UserId!), project.WorkspaceId, project.TeamId) == false)
            {
                _logger.LogError("{Handler} - User does not have access to project", nameof(GetKanbanProjectByIdHandler));

                return null;
            }

            var projectDto = project.ToKanbanProjectDto();

            _logger.LogInformation("{Handler} - Finish", nameof(GetKanbanProjectByIdHandler));

            return projectDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Handler} - Error - {Message}", nameof(GetKanbanProjectByIdHandler), ex.Message);

            return null;
        }
    }
}