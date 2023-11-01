using Portal.Common.Constants;
using Shared.Domain;

namespace Portal.Domain;

public class Project : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string? Notes { get; set; }
    public string Color { get; set; } = ColorHexCode.Default;
    public DateTime? DueDate { get; set; }
    public bool Archived { get; set; }

    public Guid OwnerId { get; set; }
    public Guid TeamId { get; set; }
    public Guid WorkspaceId { get; set; }

    public ICollection<ProjectStatus> Statues { get; set; } = default!;
    public ICollection<Section> Sections { get; set; } = default!;
    public ICollection<Task> Tasks { get; set; } = default!;
}