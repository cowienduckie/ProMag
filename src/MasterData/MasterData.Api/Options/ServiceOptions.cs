using Shared.CustomTypes;

namespace MasterData.Api.Options;

public class ServiceOptions
{
    public ServiceConfig IdentityService { get; set; } = default!;
}