using Portal.Common.Constants;
using Shared.Domain;

namespace Portal.Domain;

public class Tag : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string Color { get; set; } = ColorHexCode.Default;
    public string? Notes { get; set; }

    public ICollection<Task> Tasks { get; set; } = default!;
    public ICollection<TagFollower> Followers { get; set; } = default!;
}