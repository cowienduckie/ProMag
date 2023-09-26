using MediatR;
using Promag.Protobuf.Commons.V1;

namespace Communication.UseCases.Query;

public class PingQuery : IRequest<PongReply>
{
}