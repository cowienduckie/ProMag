using HotChocolate.Types;
using PersonalData.UseCases.Responses;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class InviteUserResponseType : ObjectType<InviteUserResponse>
{
    protected override void Configure(IObjectTypeDescriptor<InviteUserResponse> descriptor)
    {
    }
}