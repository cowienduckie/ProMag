using HotChocolate.Types;

namespace MasterData.Boundaries.GraphQl;

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor.Field("ping")
            .Type<StringType>()
            .Resolve(() => "ping");
    }
}