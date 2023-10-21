using Shared.CustomTypes;

namespace GraphQl.Gateway.Options;

public class ServiceOptions
{
    public ServiceConfig IdentityService { get; set; } = default!;
    public ServiceConfig CommunicationService { get; set; } = default!;
    public ServiceConfig PersonalService { get; set; } = default!;
    public ServiceConfig MasterDataService { get; set; } = default!;
    public ServiceConfig PortalService { get; set; } = default!;
}