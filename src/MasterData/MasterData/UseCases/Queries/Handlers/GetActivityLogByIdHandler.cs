using Mapster;
using MasterData.Boundaries.GraphQl.Dtos;
using MasterData.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace MasterData.UseCases.Queries.Handlers;

public class GetActivityLogByIdHandler : IRequestHandler<GetActivityLogByIdQuery, ActivityLogDto>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<GetActivityLogByIdHandler> _logger;

    public GetActivityLogByIdHandler(ILogger<GetActivityLogByIdHandler> logger, MasterDataDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ActivityLogDto> Handle(GetActivityLogByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetActivityLogByIdHandler));

        Guard.NotNull(request);

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetActivityLogByIdHandler));

        return await _context.ActivityLogs
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => x.Adapt<ActivityLogDto>())
            .FirstOrDefaultAsync(cancellationToken) ?? new ActivityLogDto();
    }
}