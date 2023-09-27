namespace Shared.CustomTypes;

public class ServiceConfig
{
    public string ServiceName { get; set; } = default!;
    public string ExternalUrl { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string GrpcUrl { get; set; } = default!;
}