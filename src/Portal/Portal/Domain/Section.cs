using Shared.Domain;

namespace Portal.Domain;

public class Section : AuditableEntity
{
    public Section()
    {
    }

    public Section(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = default!;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public ICollection<Task> Tasks { get; set; } = default!;
}