using MediatR;
using Portal.UseCases.Responses;

namespace Portal.UseCases.Mutations;

public class CreateTaskCommand : IRequest<CreateTaskResponse>
{
    public string Name { get; set; } = default!;
    public string? Notes { get; set; }

    public DateTime? StartOn { get; set; }
    public DateTime? DueOn { get; set; }

    public string SectionId { get; set; } = default!;
    public string ProjectId { get; set; } = default!;
}