using Communication.Common.Constants;
using Communication.EmailTemplates;
using Configuration.MassTransit.IntegrationEvents.Email;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Email;

namespace Communication.IntegrationEvents.Consumers;

public class SendAccountUnlockedEmailConsumer : IConsumer<SendAccountUnlockedEmail>
{
    private readonly IEmailClient _emailClient;
    private readonly IHandlebarsRender _handlebarsRender;
    private readonly ILogger<SendAccountUnlockedEmailConsumer> _logger;

    public SendAccountUnlockedEmailConsumer(
        IEmailClient emailClient,
        IHandlebarsRender handlebarsRender,
        ILogger<SendAccountUnlockedEmailConsumer> logger)
    {
        _emailClient = emailClient;
        _handlebarsRender = handlebarsRender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendAccountUnlockedEmail> context)
    {
        _logger.LogInformation("{ConsumerName} - Start", nameof(SendAccountUnlockedEmailConsumer));

        var bodyEmail = await _handlebarsRender.RenderAsync(EmailTemplate.AccountUnlocked, context.Message);

        if (bodyEmail is null)
        {
            _logger.LogError("{ConsumerName} - Rendered email body is NULL", nameof(SendAccountUnlockedEmailConsumer));

            return;
        }

        const string emailSubject = "Your login account has been unlocked";

        await _emailClient
            .To(context.Message.ReceiverEmail)
            .Subject(emailSubject)
            .Body(bodyEmail, true)
            .SendAsync();

        _logger.LogInformation("{ConsumerName} - Finish", nameof(SendAccountUnlockedEmailConsumer));
    }
}