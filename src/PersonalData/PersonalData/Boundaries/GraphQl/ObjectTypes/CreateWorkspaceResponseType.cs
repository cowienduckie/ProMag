using HotChocolate.Types;
using PersonalData.UseCases.Responses;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class CreateWorkspaceResponseType : ObjectType<CreateWorkspaceResponse>
{
    protected override void Configure(IObjectTypeDescriptor<CreateWorkspaceResponse> descriptor)
    {
    }
}