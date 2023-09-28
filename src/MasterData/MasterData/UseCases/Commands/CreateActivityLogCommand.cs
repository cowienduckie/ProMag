using MediatR;
using Promag.Protobuf.MasterData.V1;

namespace MasterData.UseCases.Commands;

public class CreateActivityLogCommand : IRequest<CreateActivityLogResponse>
{
    public CreateActivityLogCommand(CreateActivityLogRequest request)
    {
        Payload = request;
    }

    public CreateActivityLogRequest Payload { get; private set; }
}