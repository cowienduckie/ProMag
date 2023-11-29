using Communication.Common.Constants;
using Communication.EmailTemplates;
using Configuration.MassTransit.IntegrationEvents.Email;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Email;

namespace Communication.IntegrationEvents.Consumers;

public class SendResetPasswordEmailConsumer : IConsumer<SendResetPasswordEmail>
{
    private readonly IEmailClient _emailClient;
    private readonly IHandlebarsRender _handlebarsRender;
    private readonly ILogger<SendResetPasswordEmailConsumer> _logger;

    public SendResetPasswordEmailConsumer(
        IEmailClient emailClient,
        IHandlebarsRender handlebarsRender,
        ILogger<SendResetPasswordEmailConsumer> logger)
    {
        _emailClient = emailClient;
        _handlebarsRender = handlebarsRender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendResetPasswordEmail> context)
    {
        _logger.LogInformation("{ConsumerName} - Start", nameof(SendResetPasswordEmailConsumer));

        var bodyEmail = await _handlebarsRender.RenderAsync(EmailTemplate.ResetPassword, context.Message);

        if (bodyEmail is null)
        {
            _logger.LogError("{ConsumerName} - Rendered email body is NULL", nameof(SendResetPasswordEmailConsumer));

            return;
        }

        const string emailSubject = "Complete your password reset request";

        await _emailClient
            .To(context.Message.ReceiverEmail)
            .Subject(emailSubject)
            .Body(bodyEmail, true)
            .SendAsync();

        _logger.LogInformation("{ConsumerName} - Finish", nameof(SendResetPasswordEmailConsumer));
    }
}