using Shared.Domain;

namespace PersonalData.Domain;

public class Workspace : AuditableEntity
{
    public string Name { get; set; } = default!;
    public Guid? OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public ICollection<Person> Members { get; set; } = default!;
    public ICollection<Team> Teams { get; set; } = default!;
    public ICollection<WorkspaceInvitation> Invitations { get; set; } = default!;
}