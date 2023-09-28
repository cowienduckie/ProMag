using HotChocolate.Types;
using Humanizer;
using MasterData.Boundaries.GraphQl.InputObjectTypes;

namespace MasterData.Boundaries.GraphQl;

public class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor
            .Field(t => t.CreateActivityLog(default!))
            .Argument(CreateActivityLogInputType.InputName.Camelize(), a => a.Type<NonNullType<CreateActivityLogInputType>>());
    }
}