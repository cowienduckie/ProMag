using MediatR;
using Portal.Boundaries.GraphQL.Dtos;

namespace Portal.UseCases.Queries;

public class GetProjectsQuery : IRequest<IQueryable<SimplifiedProjectDto>>;