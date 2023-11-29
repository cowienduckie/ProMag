using MassTransit;

namespace Configuration.MassTransit.IntegrationEvents.Email;

public record SendActiveAccountEmail(
    Guid CorrelationId,
    string ReceiverEmail,
    string FullName,
    string UserName,
    string ActivateUrl
) : CorrelatedBy<Guid>;