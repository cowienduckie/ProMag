using MediatR;
using Promag.Protobuf.Commons.V1;

namespace PersonalData.UseCases.Queries;

public class PingQuery : IRequest<PongReply>;