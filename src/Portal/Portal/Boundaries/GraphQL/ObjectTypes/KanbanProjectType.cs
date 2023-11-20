using HotChocolate.Types;
using Portal.Boundaries.GraphQL.Dtos.Projects;

namespace Portal.Boundaries.GraphQL.ObjectTypes;

public class KanbanProjectType : ObjectType<KanbanProjectDto>
{
    protected override void Configure(IObjectTypeDescriptor<KanbanProjectDto> descriptor)
    {
        descriptor.Field(x => x.Tasks).Type<AnyType>();
        descriptor.Field(x => x.Columns).Type<AnyType>();
    }
}