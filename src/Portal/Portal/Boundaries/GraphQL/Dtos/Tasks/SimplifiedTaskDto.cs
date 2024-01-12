using Task = Portal.Domain.Task;

namespace Portal.Boundaries.GraphQL.Dtos.Tasks;

public class SimplifiedTaskDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public bool IsCompleted { get; set; }

    public DateTime? StartOn { get; set; }
    public DateTime? DueOn { get; set; }

    public string? Assignee { get; set; }
}

public static partial class DtoConverter
{
    public static SimplifiedTaskDto ToSimplifiedTaskDto(this Task task)
    {
        return new SimplifiedTaskDto
        {
            Id = task.Id.ToString(),
            Name = task.Name,

            IsCompleted = task.Completed,

            StartOn = task.StartOn,
            DueOn = task.DueOn,

            Assignee = task.AssigneeId.ToString()
        };
    }
}