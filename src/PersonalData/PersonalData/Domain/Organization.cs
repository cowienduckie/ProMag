using Shared.Domain;

namespace PersonalData.Domain;

public class Organization : AuditableEntity
{
    public string Name { get; set; } = default!;
    public string? EmailDomain { get; set; }
    public Workspace Workspace { get; set; } = default!;
}