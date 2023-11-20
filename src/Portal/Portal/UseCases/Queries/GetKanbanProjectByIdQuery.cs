using MediatR;
using Portal.Boundaries.GraphQL.Dtos.Projects;

namespace Portal.UseCases.Queries;

public class GetKanbanProjectByIdQuery : IRequest<KanbanProjectDto?>
{
    public string ProjectId { get; set; } = default!;
}