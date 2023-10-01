using Mapster;
using MasterData.Boundaries.GraphQl.Dtos;
using MasterData.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace MasterData.UseCases.Queries.Handlers;

public class GetActivityLogsQueryHandler : IRequestHandler<GetActivityLogsQuery, IQueryable<ActivityLogDto>>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<GetActivityLogsQueryHandler> _logger;

    public GetActivityLogsQueryHandler(ILogger<GetActivityLogsQueryHandler> logger, MasterDataDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<IQueryable<ActivityLogDto>> Handle(GetActivityLogsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetActivityLogsQueryHandler));

        Guard.NotNull(request);

        var logs = _context.ActivityLogs
            .OrderByDescending(x => x.CreatedOn)
            .AsNoTracking()
            .Select(x => x.Adapt<ActivityLogDto>())
            .AsQueryable();

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetActivityLogsQueryHandler));

        return Task.FromResult(logs);
    }
}