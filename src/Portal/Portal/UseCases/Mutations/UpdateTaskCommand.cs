using MediatR;

namespace Portal.UseCases.Mutations;

public class UpdateTaskCommand : IRequest<bool>
{
    public string TaskId { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string? Notes { get; set; }

    public bool Completed { get; set; }

    public DateTime? StartOn { get; set; }
    public DateTime? DueOn { get; set; }

    public string? AssigneeId { get; set; }

    public string SectionId { get; set; } = default!;
    public string ProjectId { get; set; } = default!;
}