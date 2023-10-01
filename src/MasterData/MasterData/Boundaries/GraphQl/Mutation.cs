using HotChocolate;
using MasterData.UseCases.Commands;
using MediatR;
using Promag.Protobuf.MasterData.V1;

namespace MasterData.Boundaries.GraphQl;

public class Mutation
{
    public async Task<CreateActivityLogResponse> CreateActivityLog(
        CreateActivityLogRequest createActivityLogInput,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new CreateActivityLogCommand(createActivityLogInput));
    }
}