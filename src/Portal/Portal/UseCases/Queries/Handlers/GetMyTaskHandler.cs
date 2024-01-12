using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Boundaries.GraphQL.Dtos.Tasks;
using Portal.Data;
using Shared.SecurityContext;

namespace Portal.UseCases.Queries.Handlers;

public class GetMyTaskHandler : IRequestHandler<GetMyTaskQuery, IEnumerable<SimplifiedTaskDto>>
{
    private readonly ILogger<GetMyTaskHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public GetMyTaskHandler(ILogger<GetMyTaskHandler> logger, ISecurityContextAccessor securityContext, PortalContext portalContext)
    {
        _logger = logger;
        _securityContext = securityContext;
        _portalContext = portalContext;
    }

    public async Task<IEnumerable<SimplifiedTaskDto>> Handle(GetMyTaskQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(GetMyTaskHandler));

        var tasks = await _portalContext.Tasks
            .Where(t => t.AssigneeId == Guid.Parse(_securityContext.UserId!) && t.DeletedOn == null)
            .Select(t => t.ToSimplifiedTaskDto())
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{Handler} - Finish", nameof(GetMyTaskHandler));

        return tasks;
    }
}