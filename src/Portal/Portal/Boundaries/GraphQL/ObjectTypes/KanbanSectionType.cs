using HotChocolate.Types;
using Portal.Boundaries.GraphQL.Dtos.Sections;

namespace Portal.Boundaries.GraphQL.ObjectTypes;

public class KanbanSectionType : ObjectType<KanbanSectionDto>
{
    protected override void Configure(IObjectTypeDescriptor<KanbanSectionDto> descriptor)
    {
    }
}