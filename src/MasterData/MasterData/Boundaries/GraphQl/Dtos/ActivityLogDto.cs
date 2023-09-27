namespace MasterData.Boundaries.GraphQl.Dtos;

public class ActivityLogDto
{
    public Guid Id { get; set; }
    public string IpAddress { get; set; } = default!;
    public string Service { get; set; } = default!;
    public string Action { get; set; } = default!;
    public long? Duration { get; set; }
    public string Parameters { get; set; } = default!;
    public string Username { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}