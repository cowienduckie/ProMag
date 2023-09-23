using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Configuration.Metrics;

public static class Extensions
{
    private const string SectionName = "Metrics";

    private static bool _initialized;

    public static IServiceCollection AddAppMetrics(this IServiceCollection services, string sectionName = SectionName)
    {
        if (_initialized)
        {
            return services;
        }

        var options = GetAppMetricOptions(services, sectionName);

        if (options is null)
        {
            return services;
        }

        _initialized = true;

        services.AddSingleton(options);

        var metricsBuilder = new MetricsBuilder().Configuration.Configure(cfg =>
        {
            var tags = options.Tags;

            if (tags == null)
            {
                return;
            }

            tags.TryGetValue("app", out _);
            tags.TryGetValue("env", out _);
            tags.Add("server", Environment.MachineName);

            foreach (var tag in tags)
            {
                cfg.GlobalTags.TryAdd(tag.Key, tag.Value);
            }
        });

        var metrics = metricsBuilder.Build();
        var metricsWebHostOptions = GetMetricsWebHostOptions(options);

        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services
            .AddMetricsTrackingMiddleware(configuration)
            .AddMetricsEndpoints(configuration)
            .AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter())
            .AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter())
            .AddMetricsReportingHostedService(metricsWebHostOptions.UnobservedTaskExceptionHandler)
            .AddMetricsTrackingMiddleware(metricsWebHostOptions.TrackingMiddlewareOptions, configuration)
            .AddMetricsEndpoints(metricsWebHostOptions.EndpointOptions, configuration)
            .AddMetrics(metrics);

        return services;
    }

    public static IApplicationBuilder UseAppMetrics(this IApplicationBuilder app)
    {
        MetricsOptions? options;

        using (var scope = app.ApplicationServices.CreateScope())
        {
            options = scope.ServiceProvider.GetService<MetricsOptions>();
        }

        return options is { Enabled: false }
            ? app
            : app
                .UseMetricsAllEndpoints()
                .UseMetricsAllMiddleware();
    }

    private static MetricsWebHostOptions GetMetricsWebHostOptions(MetricsOptions metricsOptions)
    {
        var options = new MetricsWebHostOptions();
        if (!metricsOptions.Enabled)
        {
            return options;
        }

        if (!metricsOptions.PrometheusEnabled)
        {
            return options;
        }

        options.EndpointOptions = endpointOptions =>
        {
            endpointOptions.MetricsEndpointOutputFormatter = (metricsOptions.PrometheusFormatter?.ToLowerInvariant() ?? string.Empty) switch
            {
                "protobuf" => new MetricsPrometheusProtobufOutputFormatter(),
                _ => new MetricsPrometheusTextOutputFormatter()
            };
        };

        return options;
    }

    private static MetricsOptions? GetAppMetricOptions(IServiceCollection services, string sectionName)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        return configuration?.GetOptions<MetricsOptions>(sectionName);
    }
}