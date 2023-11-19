using HotChocolate.Types;
using Portal.UseCases.Responses;

namespace Portal.Boundaries.GraphQL.ResponseTypes;

public class CreateProjectResponseType : ObjectType<CreateProjectResponse>
{
    protected override void Configure(IObjectTypeDescriptor<CreateProjectResponse> descriptor)
    {
    }
}