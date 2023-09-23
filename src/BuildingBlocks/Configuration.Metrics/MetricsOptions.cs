namespace Configuration.Metrics;

public class MetricsOptions
{
    public bool Enabled { get; set; }
    public bool PrometheusEnabled { get; set; }
    public string? PrometheusFormatter { get; set; }
    public int Interval { get; set; }
    public IDictionary<string, string>? Tags { get; set; }
}