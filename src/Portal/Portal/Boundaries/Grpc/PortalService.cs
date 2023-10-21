using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Portal.UseCases.Queries;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.Portal.V1;

namespace Portal.Boundaries.Grpc;

public class PortalService : PortalApi.PortalApiBase
{
    private readonly IMediator _mediator;

    public PortalService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
    {
        return await _mediator.Send(new PingQuery());
    }
}