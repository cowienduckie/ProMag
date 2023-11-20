using Task = Portal.Domain.Task;

namespace Portal.Boundaries.GraphQL.Dtos.Tasks;

public class KanbanTaskDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Notes { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime? CompletedOn { get; set; }
    public string? CompletedBy { get; set; }

    public DateTime? StartOn { get; set; }
    public DateTime? DueOn { get; set; }

    public bool Liked { get; set; }
    public int LikesCount { get; set; }

    public string? Assignee { get; set; }

    public string Column { get; set; } = default!;
}

public static class DtoConverter
{
    public static KanbanTaskDto ToKanbanTaskDto(this Task task)
    {
        return new KanbanTaskDto
        {
            Id = task.Id.ToString(),
            Name = task.Name,
            Notes = task.Notes,
            IsCompleted = task.Completed,
            CompletedOn = task.CompletedOn,
            CompletedBy = task.CompletedBy.ToString(),
            StartOn = task.StartOn,
            DueOn = task.DueOn,
            Liked = task.Liked,
            LikesCount = task.LikesCount,
            Assignee = task.AssigneeId.ToString(),
            Column = task.SectionId.ToString()
        };
    }
}