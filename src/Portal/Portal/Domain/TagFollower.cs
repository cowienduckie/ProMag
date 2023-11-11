namespace Portal.Domain;

public class TagFollower
{
    public Guid UserId { get; set; }

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}