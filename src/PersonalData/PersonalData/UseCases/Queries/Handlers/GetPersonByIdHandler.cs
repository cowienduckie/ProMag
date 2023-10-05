using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Common.Converters;
using PersonalData.Data;
using Shared;

namespace PersonalData.UseCases.Queries.Handlers;

public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonDto?>
{
    private readonly PersonalContext _context;
    private readonly ILogger<GetPersonByIdHandler> _logger;

    public GetPersonByIdHandler(ILogger<GetPersonByIdHandler> logger, PersonalContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<PersonDto?> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        Guard.NotNull(request);

        _logger.LogInformation("{Handler} - PersonId={PersonId} - Start", nameof(GetPersonByIdHandler), request.PersonId);

        var searchedPerson = await _context.People
            .AsNoTracking()
            .Where(p => p.Id == request.PersonId)
            .Select(p => p.ToPersonDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (searchedPerson is null)
        {
            _logger.LogWarning("{Handler} - PersonId={PersonId} - Not Found", nameof(GetPersonByIdHandler), request.PersonId);
        }

        _logger.LogInformation("{Handler} - PersonId={PersonId} - Finish", nameof(GetPersonByIdHandler), request.PersonId);

        return searchedPerson;
    }
}