using GraphQl;
using HotChocolate.Types;
using Promag.Protobuf.Identity.V1;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class RoleType : ObjectType<RoleDto>
{
    protected override void Configure(IObjectTypeDescriptor<RoleDto> descriptor)
    {
        descriptor.IgnoreProtobufMethods();
    }
}