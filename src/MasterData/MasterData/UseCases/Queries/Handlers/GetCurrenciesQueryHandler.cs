using Mapster;
using MasterData.Boundaries.GraphQl.Dtos;
using MasterData.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace MasterData.UseCases.Queries.Handlers;

public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, IList<CurrencyDto>>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<GetCurrenciesQueryHandler> _logger;

    public GetCurrenciesQueryHandler(ILogger<GetCurrenciesQueryHandler> logger, MasterDataDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IList<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetCurrenciesQueryHandler));

        Guard.NotNull(request);

        var currencies = await _context.Currencies
            .OrderBy(x => x.DisplayName)
            .AsNoTracking()
            .Select(x => x.Adapt<CurrencyDto>())
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetCurrenciesQueryHandler));

        return currencies;
    }
}