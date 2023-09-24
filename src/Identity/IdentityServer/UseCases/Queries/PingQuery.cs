using MediatR;
using Promag.Protobuf.Commons.V1;

namespace IdentityServer.UseCases.Queries;

public class PingQuery : IRequest<PongReply>
{
}