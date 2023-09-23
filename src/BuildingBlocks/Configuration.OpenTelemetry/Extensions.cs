using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Shared;

namespace Configuration.OpenTelemetry;

public static class Extensions
{
    private static bool _initialized;

    public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
    {
        if (_initialized)
        {
            return services;
        }

        _initialized = true;

        // TODO: Add GraphQL Instrumentation for HotChocolate
        // services.AddInstrumentation(o =>
        // {
        //      o.Scopes = ActivityScopes.All;
        // });

        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
            {
                var options = GetOpenTelemetryOptions(services);

                if (options is { Enabled: true })
                {
                    builder
                        .SetSampler(GetSampler(options))
                        .AddZipkinExporter(options.ServiceName, c =>
                        {
                            if (options.ZipkinEndpoint is not null)
                            {
                                c.Endpoint = new Uri(options.ZipkinEndpoint);
                            }
                        })
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddHotChocolateInstrumentation()
                        .AddConsoleExporter()
                        .AddSource("MassTransit");
                }
            });

        return services;
    }

    private static Sampler GetSampler(OpenTelemetryOptions options)
    {
        return options.Sampler
            switch
            {
                "const" => new CustomSampler(new AlwaysOnSampler()),
                _ => new AlwaysOnSampler()
            };
    }

    private static OpenTelemetryOptions? GetOpenTelemetryOptions(IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        return configuration?.GetOptions<OpenTelemetryOptions>("OpenTelemetry");
    }
}

internal class CustomSampler : Sampler
{
    private readonly List<string> _excludeRequests = new()
    {
        "/favicon.ico",
        "/health",
        "/liveness"
    };

    private readonly Sampler _sampler;

    public CustomSampler(Sampler chainedSampler)
    {
        _sampler = chainedSampler;
    }

    public new string Description { get; } = nameof(CustomSampler);

    public override SamplingResult ShouldSample(in SamplingParameters samplingParameters)
    {
        var name = samplingParameters.Name;

        if (_excludeRequests.Any(x => name == x))
        {
            return new SamplingResult(false);
        }

        return _sampler.ShouldSample(samplingParameters);
    }
}