using MediatR;
using Portal.Boundaries.GraphQL.Dtos.Tasks;

namespace Portal.UseCases.Queries;

public class GetMyTaskQuery : IRequest<IEnumerable<SimplifiedTaskDto>>
{
}