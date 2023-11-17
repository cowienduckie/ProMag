using Configuration.MassTransit;
using Configuration.OpenTelemetry;
using Configuration.OpenTelemetry.Behaviors;
using EfCore;
using EfCore.Behaviors;
using FluentValidation;
using GraphQl;
using GraphQl.Errors;
using HealthChecks.UI.Client;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Options;
using MassTransit;
using MasterData.Api.Options;
using MasterData.Boundaries.Grpc;
using MasterData.Common.Constants;
using MasterData.Data;
using MasterData.IntegrationEvents.Consumers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Shared;
using Shared.Caching;
using Shared.Common.Constants;
using Shared.CorrelationId;
using Shared.CustomTypes;
using Shared.Grpc;
using Shared.SecurityContext;
using Shared.Serialization;
using Shared.Serialization.Implementations;
using Shared.ValidationModels;

namespace MasterData.Api;

public static class Extensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCustomOpenTelemetry()
            .AddMediatR()
            .AddCorrelationId()
            .AddHealthChecks(builder.Configuration)
            .AddGrpc()
            .AddGraphQl()
            .AddCustomMassTransit(builder.Configuration)
            .AddCustomDbContext(builder.Configuration, builder.Environment)
            .AddAuthentication(builder.Configuration)
            .AddDistributedCache(builder.Configuration)
            .AddSecurityContext()
            .AddCustomSerializer<NewtonSoftService>();

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<Anchor>()
            .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCorrelationId()
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<MasterDataService>();
                    
                    endpoints.MapGraphQL();
                    
                    endpoints
                        .MapBananaCakePop()
                        .WithOptions(new GraphQLToolOptions
                        {
                            ServeMode = GraphQLToolServeMode.Embedded
                        });

                    var appOptions = app.Configuration.GetOptions<AppOptions>("App");

                    if (appOptions.HealthCheckEnabled)
                    {
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

                    endpoints.MapGet("/", async context => { await context.Response.WriteAsync(Messages.Error.CommunicateGrpcWithoutGrpcClient); });
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

        var messageBusOptions = configuration.GetOptions<MessageBusOptions>("MessageBus");

        switch (messageBusOptions.TransportType)
        {
            case MessageBusTransportType.RabbitMQ:
                services.AddSingleton<IConnection>(_ =>
                {
                    var factory = new ConnectionFactory
                    {
                        Uri = new Uri(messageBusOptions.RabbitMq.Url),
                        UserName = messageBusOptions.RabbitMq.UserName,
                        Password = messageBusOptions.RabbitMq.Password,
                        AutomaticRecoveryEnabled = true
                    };
                    return factory.CreateConnection();
                });

                hcBuilder.AddRabbitMQ(name: "masterData-rabbitmqbus-check", tags: new[] { "rabbitmqbus" });
                break;
        }

        var connString = configuration.GetConnectionString("masterData");

        if (connString is not null)
        {
            hcBuilder.AddNpgSql(connString, name: "masterData-check", tags: new[] { "masterDataDb" });
        }

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Anchor).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }

    private static IServiceCollection AddGrpc(this IServiceCollection services)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        services.AddTransient<ClientLoggerInterceptor>();

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
            options.EnableDetailedErrors = true;
        });

        return services;
    }

    private static IServiceCollection AddGraphQl(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddAuthorization()
            .AddFiltering()
            .AddSorting()
            .RegisterObjectTypes(typeof(Anchor).Assembly)
            .ModifyRequestOptions(opt => { opt.IncludeExceptionDetails = true; })
            .AddApolloTracing(TracingPreference.Always)
            .AddErrorFilter<ValidationErrorFilter>();

        return services;
    }

    private static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumersFromNamespaceContaining<Anchor>();

            var messageBusOptions = configuration.GetOptions<MessageBusOptions>("MessageBus");
            switch (messageBusOptions.TransportType)
            {
                case MessageBusTransportType.RabbitMQ:
                    x.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(new Uri(messageBusOptions.RabbitMq.Url), "/", hc =>
                        {
                            hc.Username(messageBusOptions.RabbitMq.UserName);
                            hc.Password(messageBusOptions.RabbitMq.Password);
                        });

                        ConfigureEndpoint(ctx, cfg);
                    });
                    break;
                case MessageBusTransportType.Memory:
                    x.UsingInMemory((ctx, cfg) =>
                    {
                        cfg.ConcurrentMessageLimit = messageBusOptions.Memory.TransportConcurrencyLimit;

                        ConfigureEndpoint(ctx, cfg);
                    });
                    break;
                default:
                    throw new NotSupportedException();
            }
        });

        return services;

        void ConfigureEndpoint(IRegistrationContext ctx, IBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint(QueueName.MasterData, x => { x.Consumer<SaveActivityLogConsumer>(ctx); });

            var correlationContextAccessor = services.BuildServiceProvider().GetRequiredService<ICorrelationContextAccessor>();

            cfg.ConfigureSend(x => { x.UseCorrelationId(correlationContextAccessor); });

            cfg.UseCorrelationId(correlationContextAccessor);

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource("MasterDataService")
                .ConfigureResource(resource =>
                    resource.AddService(
                        "MasterDataService",
                        serviceVersion: "1.0.0"))
                .AddConsoleExporter()
                .Build();

            if (tracerProvider is null)
            {
                return;
            }

            cfg.UseOpenTelemetryOnSend(tracerProvider);
            cfg.UseOpenTelemetryOnPublish(tracerProvider);
            cfg.UseOpenTelemetryOnConsume(tracerProvider);
        }
    }

    private static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connString = configuration.GetConnectionString("masterData");

        services.AddDbContext<MasterDataDbContext>(options =>
        {
            options.UseNpgsql(connString, opt => { opt.EnableRetryOnFailure(3); });

            options.EnableDetailedErrors();

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<DbContext>(provider => provider.GetService<MasterDataDbContext>()!);

        services.AddAuditLogs<MasterDataDbContext>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

        services.AddAuthorization(o =>
        {
            o.AddPolicy(AuthorizationPolicy.ADMIN,
                policy =>
                {
                    policy.RequireAssertion(c => c.User.IsInRole(Roles.ADMIN_ROLE_NAME)
                                                 || c.User.IsInRole(Roles.SUPER_USER_ROLE_NAME));
                });
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = serviceOptions.IdentityService.Url;
                options.RequireHttpsMetadata = false;
                options.Audience = "master-data";

                var validIssuers = new List<string>
                {
                    "http://identity-api:5101",
                    "http://localhost:5101"
                };

                if (!string.IsNullOrEmpty(serviceOptions.IdentityService.ExternalUrl))
                {
                    validIssuers.Add(serviceOptions.IdentityService.ExternalUrl);
                }

                options.TokenValidationParameters.ValidIssuers = validIssuers;
            });

        return services;
    }

    private static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCacheOptions = configuration.GetOptions<CacheOptions>("Redis");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisCacheOptions.Configuration;
            options.InstanceName = redisCacheOptions.InstanceName;
        });

        return services;
    }
}
