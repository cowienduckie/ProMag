namespace Shared.CustomTypes;

public class ServiceConfig
{
    public ServiceConfig(string serviceName, string externalUrl, string url, string grpcUrl)
    {
        ServiceName = serviceName;
        ExternalUrl = externalUrl;
        Url = url;
        GrpcUrl = grpcUrl;
    }

    public string ServiceName { get; set; }
    public string ExternalUrl { get; set; }
    public string Url { get; set; }
    public string GrpcUrl { get; set; }
}