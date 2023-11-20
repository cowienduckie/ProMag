using HotChocolate.Types;
using Portal.UseCases.Responses;

namespace Portal.Boundaries.GraphQL.ResponseTypes;

public class CreateTaskResponseType : ObjectType<CreateTaskResponse>
{
    protected override void Configure(IObjectTypeDescriptor<CreateTaskResponse> descriptor)
    {
    }
}