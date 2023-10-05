using HotChocolate;
using MasterData.Boundaries.GraphQl.InputObjectTypes;
using MasterData.UseCases.Commands;
using MediatR;
using Promag.Protobuf.MasterData.V1;

namespace MasterData.Boundaries.GraphQl;

public class Mutation
{
    public async Task<CreateActivityLogResponse> CreateActivityLog(
        [GraphQLType(typeof(CreateActivityLogInputType))]
        CreateActivityLogRequest createActivityLogInput,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new CreateActivityLogCommand(createActivityLogInput));
    }
}