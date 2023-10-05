using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Common.Converters;
using PersonalData.Data;
using Shared;

namespace PersonalData.UseCases.Queries.Handlers;

public class GetPeopleHandler : IRequestHandler<GetPeopleQuery, IQueryable<PersonDto>>
{
    private readonly PersonalContext _context;
    private readonly ILogger<GetPeopleHandler> _logger;

    public GetPeopleHandler(ILogger<GetPeopleHandler> logger, PersonalContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<IQueryable<PersonDto>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
    {
        Guard.NotNull(request);

        _logger.LogInformation("{Handler} - Start", nameof(GetPeopleHandler));

        var searchPeople = _context.People
            .AsNoTracking()
            .OrderBy(p => p.FirstName)
            .Where(p => p.UserType == request.UserType)
            .Select(p => p.ToBriefPersonDto())
            .AsQueryable();

        _logger.LogInformation("{Handler} - Finish", nameof(GetPeopleHandler));

        return Task.FromResult(searchPeople);
    }
}