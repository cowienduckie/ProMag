namespace Portal.Domain;

public class TaskFollower
{
    public Guid UserId { get; set; }

    public Guid TaskId { get; set; }
    public Task Task { get; set; } = default!;
}