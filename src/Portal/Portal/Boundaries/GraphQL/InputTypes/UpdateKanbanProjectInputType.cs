using HotChocolate.Types;
using Portal.UseCases.Mutations;

namespace Portal.Boundaries.GraphQL.InputTypes;

public class UpdateKanbanProjectInputType : InputObjectType<UpdateKanbanProjectCommand>
{
    protected override void Configure(IInputObjectTypeDescriptor<UpdateKanbanProjectCommand> descriptor)
    {
        descriptor.Field(x => x.Tasks).Type<AnyType>();
    }
}