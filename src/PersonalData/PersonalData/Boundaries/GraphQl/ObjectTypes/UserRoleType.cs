using GraphQl;
using HotChocolate.Types;
using Promag.Protobuf.Identity.V1;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class UserRoleType : ObjectType<UserRoleDto>
{
    protected override void Configure(IObjectTypeDescriptor<UserRoleDto> descriptor)
    {
        descriptor.IgnoreProtobufMethods();
        descriptor.Ignore(x => x.UserId);
    }
}