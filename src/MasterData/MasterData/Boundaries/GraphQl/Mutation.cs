using MasterData.UseCases.Commands;
using MediatR;
using Promag.Protobuf.MasterData.V1;

namespace MasterData.Boundaries.GraphQl;

public class Mutation
{
    private readonly IMediator _mediator;

    public Mutation(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<CreateActivityLogResponse> CreateActivityLog(CreateActivityLogRequest createActivityLogInput)
    {
        return await _mediator.Send(new CreateActivityLogCommand(createActivityLogInput));
    }
}