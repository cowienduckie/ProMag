using MasterData.Boundaries.GraphQl.Dtos;
using MediatR;

namespace MasterData.UseCases.Queries;

public class GetTimezonesQuery : IRequest<IList<TimezoneDto>>;