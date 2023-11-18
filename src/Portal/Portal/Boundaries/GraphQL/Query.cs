using HotChocolate;
using MediatR;
using Portal.UseCases.Queries;
using Promag.Protobuf.Commons.V1;

namespace Portal.Boundaries.GraphQL;

public class Query
{
    [GraphQLName("PortalPing")]
    public async Task<PongReply> Ping([Service] ISender mediator)
    {
        return await mediator.Send(new PingQuery());
    }
}