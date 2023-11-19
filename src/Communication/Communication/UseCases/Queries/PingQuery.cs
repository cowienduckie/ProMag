using MediatR;
using Promag.Protobuf.Commons.V1;

namespace Communication.UseCases.Queries;

public class PingQuery : IRequest<PongReply>;