using HotChocolate.Types;
using PersonalData.UseCases.Responses;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class GetWorkspacesResponseType : ObjectType<GetWorkspacesResponse>
{
    protected override void Configure(IObjectTypeDescriptor<GetWorkspacesResponse> descriptor)
    {
    }
}