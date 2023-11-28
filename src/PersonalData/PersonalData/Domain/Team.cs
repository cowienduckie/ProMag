using Shared.Domain;

namespace PersonalData.Domain;

public class Team : AuditableEntity
{
    public string Name { get; set; } = default!;
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = default!;

    public ICollection<Person> Members { get; set; } = default!;
}