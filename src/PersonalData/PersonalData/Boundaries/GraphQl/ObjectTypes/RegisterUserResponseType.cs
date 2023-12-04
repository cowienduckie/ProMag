using HotChocolate.Types;
using PersonalData.UseCases.Responses;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class RegisterUserResponseType : ObjectType<RegisterUserResponse>
{
    protected override void Configure(IObjectTypeDescriptor<RegisterUserResponse> descriptor)
    {
    }
}