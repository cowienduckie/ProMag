using Configuration.Metrics;
using Configuration.OpenTelemetry;
using Gateways.Options;
using Gateways.Schemas;
using GraphQl.Errors;
using HealthChecks.UI.Client;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared;
using Shared.CorrelationId;
using Shared.CustomTypes;

namespace Gateways;

public static class Extensions
{
    private const string CORS_POLICY = nameof(CORS_POLICY);

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCors(builder.Configuration)
            .AddCustomOpenTelemetry()
            .AddCorrelationId()
            .AddHeaderPropagation()
            .AddAppMetrics()
            .AddHealthChecks(builder.Configuration)
            .AddGraphQl(builder.Configuration)
            .AddYarpReverseProxy(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        var pathBase = app.Configuration["PathBase"]!;

        app.UsePathBase(pathBase)
            .UseCors(CORS_POLICY)
            .UseCorrelationId()
            .UseHeaderPropagation()
            .UseAppMetrics()
            .UseRouting()
            .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                    endpoints
                        .MapBananaCakePop()
                        .WithOptions(new GraphQLToolOptions
                        {
                            ServeMode = GraphQLToolServeMode.Embedded
                        });

                    endpoints.MapReverseProxy();

                    endpoints.MapGet(pathBase, context =>
                    {
                        context.Response.Redirect($"{pathBase}/graphql");

                        return Task.CompletedTask;
                    });

                    var appOptions = app.Configuration.GetOptions<AppOptions>("App");

                    if (!appOptions.HealthCheckEnabled)
                    {
                        return;
                    }

                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                    endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                    {
                        Predicate = r => r.Name.Contains("self")
                    });
                }
            );

        return app;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetOptions<AppOptions>("App");

        if (!appOptions.HealthCheckEnabled)
        {
            return services;
        }

        var hcBuilder = services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

        hcBuilder
            .AddUrlGroup(new Uri($"{serviceOptions.CommunicationService.Url}/health"), "communicationapi-check", tags: new[] { "communicationapi" })
            .AddUrlGroup(new Uri($"{serviceOptions.IdentityService.Url}/health"), "identityapi-check", tags: new[] { "idsvrapi" })
            .AddUrlGroup(new Uri($"{serviceOptions.PersonalService.Url}/health"), "personalapi-check", tags: new[] { "personalapi" })
            .AddUrlGroup(new Uri($"{serviceOptions.MasterDataService.Url}/health"), "masterdataapi-check", tags: new[] { "masterdataapi" })
            .AddUrlGroup(new Uri($"{serviceOptions.PortalService.Url}/health"), "portalapi-check", tags: new[] { "portalapi" });

        return services;
    }

    private static IServiceCollection AddGraphQl(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

        services.AddHttpContextAccessor();

        services
            .AddHttpClient(WellKnownSchemaNames.PersonalData,
                (_, client) => { client.BaseAddress = new Uri($"{serviceOptions.PersonalService.Url}/graphql"); })
            .AddHeaderPropagation();
        services
            .AddHttpClient(WellKnownSchemaNames.MasterData,
                (_, client) => { client.BaseAddress = new Uri($"{serviceOptions.MasterDataService.Url}/graphql"); })
            .AddHeaderPropagation();
        services
            .AddHttpClient(WellKnownSchemaNames.Portal,
                (_, client) => { client.BaseAddress = new Uri($"{serviceOptions.PortalService.Url}/graphql"); })
            .AddHeaderPropagation();

        services
            .AddGraphQLServer()
            .AddErrorFilter<ValidationErrorFilter>()
            .AddRemoteSchema(WellKnownSchemaNames.PersonalData)
            .AddRemoteSchema(WellKnownSchemaNames.MasterData)
            .AddRemoteSchema(WellKnownSchemaNames.Portal);

        return services;
    }

    private static IServiceCollection AddHeaderPropagation(this IServiceCollection services)
    {
        services.AddHeaderPropagation(options =>
        {
            options.Headers.Add("X-Correlation-ID");
            options.Headers.Add("Authorization");
        });

        return services;
    }

    private static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var cors = configuration.GetValue<string>("Cors:Origins")!.Split(',');

            options.AddPolicy(CORS_POLICY,
                policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(cors)
                        .SetIsOriginAllowed(_ => true);
                });
        });

        return services;
    }

    private static IServiceCollection AddYarpReverseProxy(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));

        return services;
    }
}