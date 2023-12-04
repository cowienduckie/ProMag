using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalData.Data;
using PersonalData.UseCases.Queries;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.PersonalData.V1;

namespace PersonalData.Boundaries.Grpc;

public class PersonalService : PersonalApi.PersonalApiBase
{
    private readonly PersonalContext _context;
    private readonly IMediator _mediator;

    public PersonalService(IMediator mediator, PersonalContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
    {
        return await _mediator.Send(new PingQuery());
    }

    public override async Task<GetUserCollaborationResponse> GetUserCollaboration(GetUserCollaborationRequest request, ServerCallContext context)
    {
        var user = await _context.People
            .Include(p => p.Workspaces)
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.UserId), context.CancellationToken);

        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        var response = new GetUserCollaborationResponse();

        response.Workspaces.AddRange(user.Workspaces.Select(w => w.Id.ToString()));
        response.Teams.AddRange(user.Teams.Select(t => t.Id.ToString()));

        return response;
    }
}