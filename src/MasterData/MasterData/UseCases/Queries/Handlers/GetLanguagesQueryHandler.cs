using Mapster;
using MasterData.Boundaries.GraphQl.Dtos;
using MasterData.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace MasterData.UseCases.Queries.Handlers;

public class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, IList<LanguageDto>>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<GetLanguagesQueryHandler> _logger;

    public GetLanguagesQueryHandler(ILogger<GetLanguagesQueryHandler> logger, MasterDataDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IList<LanguageDto>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetLanguagesQueryHandler));

        Guard.NotNull(request);

        var languages = await _context.Languages
            .OrderBy(x => x.DisplayName)
            .AsNoTracking()
            .Select(x => x.Adapt<LanguageDto>())
            .ToListAsync(cancellationToken);

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetLanguagesQueryHandler));

        return languages;
    }
}