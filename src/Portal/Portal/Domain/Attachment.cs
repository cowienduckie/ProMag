using Shared.Domain;

namespace Portal.Domain;

public class Attachment : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string? Host { get; set; }
    public string? ViewUrl { get; set; }
    public string? DownloadUrl { get; set; }

    public Guid ParentId { get; set; }
    public Task ParentTask { get; set; } = default!;
}