using Portal.Domain;

namespace Portal.Boundaries.GraphQL.Dtos.Projects;

public class SimplifiedProjectDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Notes { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string WorkspaceId { get; set; } = default!;
}

public static partial class DtoConverter
{
    public static SimplifiedProjectDto ToSimplifiedProjectDto(this Project project)
    {
        return new SimplifiedProjectDto
        {
            Id = project.Id.ToString(),
            Name = project.Name,
            Notes = project.Notes,
            CreatedOn = project.CreatedOn,
            LastModifiedOn = project.LastModifiedOn,
            WorkspaceId = project.WorkspaceId.ToString()
        };
    }
}