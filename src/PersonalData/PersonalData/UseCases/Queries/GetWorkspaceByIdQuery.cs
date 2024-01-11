using MediatR;
using PersonalData.Boundaries.GraphQl.Dtos;

namespace PersonalData.UseCases.Queries;

public class GetWorkspaceByIdQuery : IRequest<WorkspaceDto?>
{
    public string WorkspaceId { get; set; } = default!;
}