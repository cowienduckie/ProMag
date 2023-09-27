using HotChocolate.Types;
using Promag.Protobuf.MasterData.V1;
using Shared.Common.Extensions;

namespace MasterData.Boundaries.GraphQl.InputObjectTypes;

public class CreateActivityLogInputType : InputObjectType<CreateActivityLogRequest>
{
    public static readonly string InputName = nameof(CreateActivityLogInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<CreateActivityLogRequest> descriptor)
    {
        descriptor.Name(InputName);
    }
}