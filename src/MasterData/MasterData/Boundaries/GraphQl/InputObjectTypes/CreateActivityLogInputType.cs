using HotChocolate.Types;
using Promag.Protobuf.MasterData.V1;
using Shared.Common.Helpers;

namespace MasterData.Boundaries.GraphQl.InputObjectTypes;

public class CreateActivityLogInputType : InputObjectType<CreateActivityLogRequest>
{
    private static readonly string _inputName = nameof(CreateActivityLogInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<CreateActivityLogRequest> descriptor)
    {
        descriptor.Name(_inputName);
    }
}