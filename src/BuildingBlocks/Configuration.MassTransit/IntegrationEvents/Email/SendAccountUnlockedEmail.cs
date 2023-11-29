using MassTransit;

namespace Configuration.MassTransit.IntegrationEvents.Email;

public record SendAccountUnlockedEmail(
    Guid CorrelationId,
    string ReceiverEmail,
    string FullName,
    string UserName,
    string ResetPasswordUrl
) : CorrelatedBy<Guid>;