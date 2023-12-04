using MediatR;
using Portal.UseCases.Responses;

namespace Portal.UseCases.Mutations;

public class AssignTaskCommand : IRequest<AssignTaskResponse>
{
    public string ProjectId { get; set; } = default!;
    public string TaskId { get; set; } = default!;
    public string AssigneeId { get; set; } = default!;
}