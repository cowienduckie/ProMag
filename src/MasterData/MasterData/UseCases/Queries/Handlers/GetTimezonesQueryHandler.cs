using Mapster;
using MasterData.Boundaries.GraphQl.Dtos;
using MasterData.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace MasterData.UseCases.Queries.Handlers;

public class GetTimezonesQueryHandler : IRequestHandler<GetTimezonesQuery, IList<TimezoneDto>>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<GetTimezonesQueryHandler> _logger;

    public GetTimezonesQueryHandler(MasterDataDbContext context, ILogger<GetTimezonesQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IList<TimezoneDto>> Handle(GetTimezonesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetTimezonesQueryHandler));

        Guard.NotNull(request);

        var timezones = await _context.Timezones
            .OrderBy(x => x.DisplayName)
            .AsNoTracking()
            .Select(x => x.Adapt<TimezoneDto>())
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetTimezonesQueryHandler));

        return timezones;
    }
}