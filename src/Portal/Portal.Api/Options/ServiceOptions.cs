using Shared.CustomTypes;

namespace Portal.Api.Options;

public class ServiceOptions
{
    public ServiceConfig IdentityService { get; set; } = default!;

    public ServiceConfig MasterDataService { get; set; } = default!;

    public ServiceConfig PersonalDataService { get; set; } = default!;
}