namespace Shared.Caching;

public class CacheOptions
{
    public CacheOptions()
    {
        Configuration = string.Empty;
        InstanceName = string.Empty;
    }

    public CacheOptions(bool enabled, string configuration, string instanceName, int slidingExpirationInSecond)
    {
        Enabled = enabled;
        Configuration = configuration;
        InstanceName = instanceName;
        SlidingExpirationInSecond = slidingExpirationInSecond;
    }

    public bool Enabled { get; set; }
    public string Configuration { get; set; }
    public string InstanceName { get; set; }
    public int SlidingExpirationInSecond { get; set; }
}