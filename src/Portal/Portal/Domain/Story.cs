using Portal.Common.Constants;
using Shared.Domain;

namespace Portal.Domain;

public class Story : AuditableEntity
{
    public string Text { get; set; } = default!;
    public string Source { get; set; } = ActionSource.System;

    public bool Liked { get; set; }
    public int LikesCount { get; set; }
    public string Likes { get; set; } = JsonString.EmptyArray;

    public Guid TargetId { get; set; }
    public Task TargetTask { get; set; } = default!;
}