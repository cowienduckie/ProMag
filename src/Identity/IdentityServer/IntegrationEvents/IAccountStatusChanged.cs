using MassTransit;
using Promag.Protobuf.Commons.V1;
using Shared.CustomTypes;

namespace IdentityServer.IntegrationEvents;

public interface IAccountStatusChanged : CorrelatedBy<Guid>, IMessage
{
    string UserId { get; set; }
    UserStatus UserStatus { get; set; }
}