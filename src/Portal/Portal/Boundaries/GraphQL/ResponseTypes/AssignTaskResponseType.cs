using HotChocolate.Types;
using Portal.UseCases.Responses;

namespace Portal.Boundaries.GraphQL.ResponseTypes;

public class AssignTaskResponseType : ObjectType<AssignTaskResponse>
{
    protected override void Configure(IObjectTypeDescriptor<AssignTaskResponse> descriptor)
    {
    }
}