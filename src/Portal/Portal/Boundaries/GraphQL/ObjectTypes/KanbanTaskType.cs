using HotChocolate.Types;
using Portal.Boundaries.GraphQL.Dtos.Tasks;

namespace Portal.Boundaries.GraphQL.ObjectTypes;

public class KanbanTaskType : ObjectType<KanbanTaskDto>
{
    protected override void Configure(IObjectTypeDescriptor<KanbanTaskDto> descriptor)
    {
    }
}