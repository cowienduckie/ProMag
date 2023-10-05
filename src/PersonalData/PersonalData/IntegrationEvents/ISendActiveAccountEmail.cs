using MassTransit;
using Shared.CustomTypes;

namespace PersonalData.IntegrationEvents;

public interface ISendActiveAccountEmail : CorrelatedBy<Guid>, IMessage
{
    string ReceiverEmail { get; set; }
    string FullName { get; set; }
    string UserName { get; set; }
    string ActivateUrl { get; set; }
}