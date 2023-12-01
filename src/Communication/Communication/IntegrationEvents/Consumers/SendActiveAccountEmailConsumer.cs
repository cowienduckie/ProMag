using Communication.Common.Constants;
using Communication.EmailRender;
using Configuration.MassTransit.IntegrationEvents.Email;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Email;

namespace Communication.IntegrationEvents.Consumers;

public class SendActiveAccountEmailConsumer : IConsumer<SendActiveAccountEmail>
{
    private readonly IEmailClient _emailClient;
    private readonly IHandlebarsRender _handlebarsRender;
    private readonly ILogger<SendActiveAccountEmailConsumer> _logger;

    public SendActiveAccountEmailConsumer(
        IEmailClient emailClient,
        IHandlebarsRender handlebarsRender,
        ILogger<SendActiveAccountEmailConsumer> logger)
    {
        _emailClient = emailClient;
        _handlebarsRender = handlebarsRender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendActiveAccountEmail> context)
    {
        _logger.LogInformation("{ConsumerName} - Start", nameof(SendActiveAccountEmailConsumer));

        var bodyEmail = await _handlebarsRender.RenderAsync(EmailTemplate.Activation, context.Message);

        if (bodyEmail is null)
        {
            _logger.LogError("{ConsumerName} - Rendered email body is NULL", nameof(SendActiveAccountEmailConsumer));

            return;
        }

        var emailSubject = $"Hi {context.Message.FullName}, Activate your ProMag account";

        await _emailClient
            .To(context.Message.ReceiverEmail)
            .Subject(emailSubject)
            .Body(bodyEmail, true)
            .SendAsync();

        _logger.LogInformation("{ConsumerName} - Finish", nameof(SendActiveAccountEmailConsumer));
    }
}