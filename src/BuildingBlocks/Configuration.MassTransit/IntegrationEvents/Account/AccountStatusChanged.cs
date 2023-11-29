using MassTransit;
using Shared.Common.Enums;

namespace Configuration.MassTransit.IntegrationEvents.Account;

public record AccountStatusChanged(
    Guid CorrelationId,
    string UserId,
    UserStatus UserStatus
) : CorrelatedBy<Guid>;