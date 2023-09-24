using MassTransit;
using Shared.CustomTypes;

namespace IdentityServer.IntegrationEvents;

public interface ISendResetPasswordEmail : CorrelatedBy<Guid>, IMessage
{
    string ResetPasswordLink { get; set; }
    string ReceiverEmail { get; set; }
    string UserName { get; set; }
}