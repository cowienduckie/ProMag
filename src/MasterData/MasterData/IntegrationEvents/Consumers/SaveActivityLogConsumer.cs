using Configuration.MassTransit.IntegrationEvents.Logging;
using MassTransit;
using MediatR;
using Promag.Protobuf.MasterData.V1;

namespace MasterData.IntegrationEvents.Consumers;

public class SaveActivityLogConsumer : IConsumer<SaveActivityLog>
{
    private readonly IMediator _mediator;

    public SaveActivityLogConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<SaveActivityLog> context)
    {
        var createActivityLogInput = new CreateActivityLogRequest
        {
            IpAddress = context.Message.IpAddress,
            Service = context.Message.Service,
            Action = context.Message.Action,
            Duration = context.Message.Duration,
            Parameters = context.Message.Parameters,
            Username = context.Message.Username
        };

        await _mediator.Send(createActivityLogInput);
    }
}