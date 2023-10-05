using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using PersonalData.UseCases.Queries;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.PersonalData.V1;

namespace PersonalData.Boundaries.Grpc;

public class PersonalService : PersonalApi.PersonalApiBase
{
    private readonly IMediator _mediator;

    public PersonalService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
    {
        return await _mediator.Send(new PingQuery());
    }
}