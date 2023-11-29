using MassTransit;

namespace Configuration.MassTransit.IntegrationEvents.Email;

public record SendResetPasswordEmail(
    Guid CorrelationId,
    string ResetPasswordLink,
    string ReceiverEmail,
    string UserName
) : CorrelatedBy<Guid>;