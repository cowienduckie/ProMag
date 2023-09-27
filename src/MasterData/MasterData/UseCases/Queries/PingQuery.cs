using MediatR;
using Promag.Protobuf.Commons.V1;

namespace MasterData.UseCases.Queries;

public class PingQuery : IRequest<PongReply>
{
}