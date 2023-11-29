using MassTransit;

namespace Configuration.MassTransit.IntegrationEvents.Logging;

public record SaveActivityLog(
    Guid CorrelationId,
    string IpAddress,
    string Service,
    string Action,
    long? Duration,
    string Parameters,
    string Username
) : CorrelatedBy<Guid>;