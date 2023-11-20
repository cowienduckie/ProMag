using MediatR;
using Portal.Boundaries.GraphQL.Dtos.Projects;

namespace Portal.UseCases.Queries;

public class GetProjectsQuery : IRequest<IQueryable<SimplifiedProjectDto>>;