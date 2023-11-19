using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Boundaries.GraphQL.Dtos;
using Portal.Data;
using Shared.SecurityContext;

namespace Portal.UseCases.Queries.Handlers;

public class GetProjectsHandler : IRequestHandler<GetProjectsQuery, IQueryable<SimplifiedProjectDto>>
{
    private readonly ILogger<GetProjectsHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public GetProjectsHandler(ILogger<GetProjectsHandler> logger, PortalContext portalContext, ISecurityContextAccessor securityContext)
    {
        _logger = logger;
        _portalContext = portalContext;
        _securityContext = securityContext;
    }

    public Task<IQueryable<SimplifiedProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetProjectsHandler));

        if (!Guid.TryParse(_securityContext.UserId, out var userId))
        {
            _logger.LogError("{HandlerName} - UserId is not a valid Guid", nameof(GetProjectsHandler));

            return Task.FromResult(Enumerable.Empty<SimplifiedProjectDto>().AsQueryable());
        }

        var projectDtos = _portalContext.Projects
            .Where(p => p.DeletedOn == null
                        && p.OwnerId == userId)
            .OrderBy(p => p.Name)
            .Select(p => p.ToSimplifiedProjectDto())
            .AsQueryable();

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetProjectsHandler));

        return Task.FromResult(projectDtos);
    }
}