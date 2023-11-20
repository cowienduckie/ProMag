using HotChocolate.Types;
using Portal.UseCases.Responses;

namespace Portal.Boundaries.GraphQL.ResponseTypes;

public class UpdateKanbanProjectResponseType : ObjectType<UpdateKanbanProjectResponse>
{
    protected override void Configure(IObjectTypeDescriptor<UpdateKanbanProjectResponse> descriptor)
    {
    }
}