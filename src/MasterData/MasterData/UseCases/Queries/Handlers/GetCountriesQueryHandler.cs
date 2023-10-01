using Mapster;
using MasterData.Boundaries.GraphQl.Dtos;
using MasterData.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace MasterData.UseCases.Queries.Handlers;

public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IList<CountryDto>>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<GetCountriesQueryHandler> _logger;

    public GetCountriesQueryHandler(ILogger<GetCountriesQueryHandler> logger, MasterDataDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IList<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetCountriesQueryHandler));

        Guard.NotNull(request);

        var countries = await _context.Countries
            .OrderBy(x => x.DisplayName)
            .AsNoTracking()
            .Select(x => x.Adapt<CountryDto>())
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetCountriesQueryHandler));

        return countries;
    }
}