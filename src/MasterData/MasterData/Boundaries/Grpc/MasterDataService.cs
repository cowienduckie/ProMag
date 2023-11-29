using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MasterData.UseCases.Commands;
using MasterData.UseCases.Queries;
using MediatR;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.MasterData.V1;

namespace MasterData.Boundaries.Grpc;

public class MasterDataService : MasterDataApi.MasterDataApiBase
{
    private readonly IMediator _mediator;

    public MasterDataService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
    {
        return await _mediator.Send(new PingQuery());
    }

    public override async Task<CreateActivityLogResponse> CreateActivityLog(CreateActivityLogRequest request, ServerCallContext context)
    {
        return await _mediator.Send(new CreateActivityLogCommand(request));
    }
}