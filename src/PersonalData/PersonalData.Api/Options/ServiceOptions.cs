using Shared.CustomTypes;

namespace PersonalData.Api.Options;

public class ServiceOptions
{
    public ServiceConfig IdentityService { get; set; } = default!;

    public ServiceConfig MasterDataService { get; set; } = default!;
}