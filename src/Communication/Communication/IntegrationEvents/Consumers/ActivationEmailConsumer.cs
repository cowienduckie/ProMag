using Communication.Common.Constants;
using Communication.EmailTemplates;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Email;

namespace Communication.IntegrationEvents.Consumers;

public class ActivationEmailConsumer : IConsumer<IActivationEmail>
{
    private readonly IEmailClient _emailClient;
    private readonly IHandlebarsRender _handlebarsRender;
    private readonly ILogger<ActivationEmailConsumer> _logger;

    public ActivationEmailConsumer(
        IEmailClient emailClient,
        IHandlebarsRender handlebarsRender,
        ILogger<ActivationEmailConsumer> logger)
    {
        _emailClient = emailClient;
        _handlebarsRender = handlebarsRender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IActivationEmail> context)
    {
        _logger.LogInformation("{ConsumerName} - Start", nameof(ActivationEmailConsumer));

        var bodyEmail = await _handlebarsRender.RenderAsync(EmailTemplate.Activation, context.Message);

        if (bodyEmail is null)
        {
            _logger.LogError("{ConsumerName} - Rendered email body is NULL", nameof(ActivationEmailConsumer));

            return;
        }

        var emailSubject = $"Hi {context.Message.FullName}, Activate your ProMag account";

        await _emailClient
            .To(context.Message.ReceiverEmail)
            .Subject(emailSubject)
            .Body(bodyEmail, true)
            .SendAsync();

        _logger.LogInformation("{ConsumerName} - Finish", nameof(ActivationEmailConsumer));
    }
}