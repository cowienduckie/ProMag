using GraphQl;
using HotChocolate.Types;
using Promag.Protobuf.Identity.V1;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class PermissionType : ObjectType<RoleClaimsResponse>
{
    protected override void Configure(IObjectTypeDescriptor<RoleClaimsResponse> descriptor)
    {
        descriptor.IgnoreProtobufMethods();
    }
}