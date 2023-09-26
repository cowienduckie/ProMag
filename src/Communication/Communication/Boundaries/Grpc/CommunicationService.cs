using Communication.UseCases.Query;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.Communication.V1;

namespace Communication.Boundaries.Grpc;

public class CommunicationService : CommunicationApi.CommunicationApiBase
{
    private readonly IMediator _mediator;

    public CommunicationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
    {
        var result = await _mediator.Send(new PingQuery());

        return result;
    }
}