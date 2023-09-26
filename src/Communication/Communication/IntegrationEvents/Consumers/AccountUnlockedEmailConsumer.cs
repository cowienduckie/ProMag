using Communication.Common.Constants;
using Communication.EmailTemplates;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Email;

namespace Communication.IntegrationEvents.Consumers;

public class AccountUnlockedEmailConsumer : IConsumer<IAccountUnlockedEmail>
{
    private readonly IEmailClient _emailClient;
    private readonly IHandlebarsRender _handlebarsRender;
    private readonly ILogger<AccountUnlockedEmailConsumer> _logger;

    public AccountUnlockedEmailConsumer(
        IEmailClient emailClient,
        IHandlebarsRender handlebarsRender,
        ILogger<AccountUnlockedEmailConsumer> logger)
    {
        _emailClient = emailClient;
        _handlebarsRender = handlebarsRender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IAccountUnlockedEmail> context)
    {
        _logger.LogInformation("{ConsumerName} - Start", nameof(AccountUnlockedEmailConsumer));

        var bodyEmail = await _handlebarsRender.RenderAsync(EmailTemplate.AccountUnlocked, context.Message);

        if (bodyEmail is null)
        {
            _logger.LogError("{ConsumerName} - Rendered email body is NULL", nameof(AccountUnlockedEmailConsumer));

            return;
        }

        const string emailSubject = "Your login account has been unlocked";

        await _emailClient
            .To(context.Message.ReceiverEmail)
            .Subject(emailSubject)
            .Body(bodyEmail, true)
            .SendAsync();

        _logger.LogInformation("{ConsumerName} - Finish", nameof(AccountUnlockedEmailConsumer));
    }
}