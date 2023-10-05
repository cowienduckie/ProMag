using MassTransit;
using PersonalData.Common.Enums;
using Shared.CustomTypes;

namespace PersonalData.IntegrationEvents;

public interface IAccountStatusChanged : CorrelatedBy<Guid>, IMessage
{
    string UserId { get; set; }
    UserStatus UserStatus { get; set; }
}