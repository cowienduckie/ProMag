using Portal.Common.Constants;
using Shared.Domain;

namespace Portal.Domain;

public class ProjectStatus : AuditableEntity
{
    public string Title { get; set; } = default!;
    public string? Text { get; set; }
    public string Color { get; set; } = ColorHexCode.Default;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
}