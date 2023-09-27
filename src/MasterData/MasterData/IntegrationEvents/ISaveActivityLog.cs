using MassTransit;
using Shared.CustomTypes;

namespace MasterData.IntegrationEvents;

public interface ISaveActivityLog : CorrelatedBy<Guid>, IMessage
{
    string IpAddress { get; set; }
    string Service { get; set; }
    string Action { get; set; }
    long? Duration { get; set; }
    string Parameters { get; set; }
    string Username { get; set; }
}