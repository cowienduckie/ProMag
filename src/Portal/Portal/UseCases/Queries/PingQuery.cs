using MediatR;
using Promag.Protobuf.Commons.V1;

namespace Portal.UseCases.Queries;

public class PingQuery : IRequest<PongReply>
{
}