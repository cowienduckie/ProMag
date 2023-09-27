using MasterData.Boundaries.GraphQl.Dtos;
using MediatR;

namespace MasterData.UseCases.Queries;

public class GetTimezoneQuery : IRequest<IList<TimezoneDto>>
{
}