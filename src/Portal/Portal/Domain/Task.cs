using Portal.Common.Constants;
using Shared.Domain;

namespace Portal.Domain;

public class Task : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string? Notes { get; set; }

    public bool Completed { get; set; }
    public DateTime? CompletedOn { get; set; }
    public Guid? CompletedBy { get; set; }

    public DateTime? StartOn { get; set; }
    public DateTime? DueOn { get; set; }

    public bool Liked { get; set; }
    public int LikesCount { get; set; }
    public string Likes { get; set; } = JsonString.EmptyArray;

    public string CustomFields { get; set; } = JsonString.EmptyArray;

    public Guid WorkspaceId { get; set; }
    public Guid? AssigneeId { get; set; }

    public Guid SectionId { get; set; }
    public Section Section { get; set; } = default!;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public ICollection<Story> Stories { get; set; } = default!;
    public ICollection<Attachment> Attachments { get; set; } = default!;
    public ICollection<Tag> Tags { get; set; } = default!;
    public ICollection<TaskFollower> Followers { get; set; } = default!;
}