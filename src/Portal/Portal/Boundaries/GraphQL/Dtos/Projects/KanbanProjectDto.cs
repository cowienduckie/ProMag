using Portal.Boundaries.GraphQL.Dtos.Sections;
using Portal.Boundaries.GraphQL.Dtos.Tasks;
using Portal.Domain;

namespace Portal.Boundaries.GraphQL.Dtos.Projects;

public class KanbanProjectDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Notes { get; set; }
    public string Color { get; set; } = default!;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string WorkspaceId { get; set; } = default!;

    public IReadOnlyDictionary<string, object> Tasks { get; set; } = default!;
    public IReadOnlyDictionary<string, object> Columns { get; set; } = default!;

    public List<string> ColumnOrder { get; set; } = default!;
}

public static partial class DtoConverter
{
    public static KanbanProjectDto ToKanbanProjectDto(this Project project)
    {
        var columnOrder = project.Sections
            .OrderBy(s => s.OrderIndex)
            .Select(s => s.Id.ToString())
            .ToList();

        var tasks = project.Tasks
            .OrderByDescending(t => t.DueOn)
            .ThenByDescending(t => t.LastModifiedOn)
            .Select(t => t.ToKanbanTaskDto())
            .ToDictionary(t => t.Id, t => t as object);

        var columns = project.Sections
            .Select(s => s.ToKanbanSectionDto())
            .ToDictionary(s => s.Id, t => t as object);

        return new KanbanProjectDto
        {
            Id = project.Id.ToString(),
            Name = project.Name,
            Notes = project.Notes,
            Color = project.Color,
            DueDate = project.DueDate,
            CreatedOn = project.CreatedOn,
            LastModifiedOn = project.LastModifiedOn,
            WorkspaceId = project.WorkspaceId.ToString(),

            Tasks = tasks,
            Columns = columns,

            ColumnOrder = columnOrder
        };
    }
}