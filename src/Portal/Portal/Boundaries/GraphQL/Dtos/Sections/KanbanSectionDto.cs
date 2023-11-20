using Portal.Domain;

namespace Portal.Boundaries.GraphQL.Dtos.Sections;

public class KanbanSectionDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<string> TaskIds { get; set; } = default!;
}

public static class DtoConverter
{
    public static KanbanSectionDto ToKanbanSectionDto(this Section section)
    {
        var taskIds = section.Tasks.Select(t => t.Id.ToString()).ToList();

        return new KanbanSectionDto
        {
            Id = section.Id.ToString(),
            Name = section.Name,
            TaskIds = taskIds
        };
    }
}