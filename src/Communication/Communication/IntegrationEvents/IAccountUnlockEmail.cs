using MassTransit;
using Shared.CustomTypes;

namespace Communication.IntegrationEvents;

public interface IAccountUnlockedEmail : CorrelatedBy<Guid>, IMessage
{
    string ReceiverEmail { get; set; }
    string FullName { get; set; }
    string UserName { get; set; }
    string ResetPasswordUrl { get; set; }
}