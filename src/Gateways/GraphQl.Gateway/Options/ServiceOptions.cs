using Shared.CustomTypes;

namespace GraphQl.Gateway.Options;

public class ServiceOptions
{
    public ServiceConfig SaleService { get; set; }
    public ServiceConfig IdentityService { get; set; }
    public ServiceConfig CommunicationService { get; set; }
    public ServiceConfig PersonalService { get; set; }
    public ServiceConfig MasterDataService { get; set; }
}