using Shared.Domain;

namespace PersonalData.Domain;

public class WorkspaceInvitation : AuditableEntity
{
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = default!;

    public Guid InvitedPersonId { get; set; }
    public Person InvitedPerson { get; set; } = default!;

    public bool Accepted { get; set; } = false;
}