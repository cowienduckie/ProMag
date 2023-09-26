using Communication.Common.Constants;
using Communication.EmailTemplates;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Email;

namespace Communication.IntegrationEvents.Consumers;

public class ResetPasswordEmailConsumer : IConsumer<IResetPasswordEmail>
{
    private readonly IEmailClient _emailClient;
    private readonly IHandlebarsRender _handlebarsRender;
    private readonly ILogger<ResetPasswordEmailConsumer> _logger;

    public ResetPasswordEmailConsumer(
        IEmailClient emailClient,
        IHandlebarsRender handlebarsRender,
        ILogger<ResetPasswordEmailConsumer> logger)
    {
        _emailClient = emailClient;
        _handlebarsRender = handlebarsRender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IResetPasswordEmail> context)
    {
        _logger.LogInformation("{ConsumerName} - Start", nameof(ResetPasswordEmailConsumer));

        var bodyEmail = await _handlebarsRender.RenderAsync(EmailTemplate.ResetPassword, context.Message);

        if (bodyEmail is null)
        {
            _logger.LogError("{ConsumerName} - Rendered email body is NULL", nameof(ResetPasswordEmailConsumer));

            return;
        }

        const string emailSubject = "Complete your password reset request";

        await _emailClient
            .To(context.Message.ReceiverEmail)
            .Subject(emailSubject)
            .Body(bodyEmail, true)
            .SendAsync();

        _logger.LogInformation("{ConsumerName} - Finish", nameof(ResetPasswordEmailConsumer));
    }
}