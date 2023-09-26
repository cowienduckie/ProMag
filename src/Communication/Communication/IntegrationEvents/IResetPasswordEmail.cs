using MassTransit;
using Shared.CustomTypes;

namespace Communication.IntegrationEvents;

public interface IResetPasswordEmail : CorrelatedBy<Guid>, IMessage
{
    string UserName { get; set; }
    string ResetPasswordLink { get; set; }
    string ReceiverEmail { get; set; }
}