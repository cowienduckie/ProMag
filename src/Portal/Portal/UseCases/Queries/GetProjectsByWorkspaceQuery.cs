using MediatR;
using Portal.Boundaries.GraphQL.Dtos.Projects;

namespace Portal.UseCases.Queries;

public class GetProjectsByWorkspaceQuery : IRequest<IEnumerable<SimplifiedProjectDto>>
{
    public string WorkspaceId { get; set; } = default!;
}