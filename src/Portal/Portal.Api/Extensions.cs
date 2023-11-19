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
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Portal.Api.Options;
using Portal.Boundaries.Grpc;
using Portal.Data;
using Portal.Data.Audit;
using Promag.Protobuf.MasterData.V1;
using RabbitMQ.Client;
using Shared;
using Shared.Common.Constants;
using Shared.CorrelationId;
using Shared.CustomTypes;
using Shared.Grpc;
using Shared.SecurityContext;
using Shared.Serialization;
using Shared.Serialization.Implementations;
using Shared.Storage;
using Shared.Storage.Extensions;
using Shared.Storage.FileSystem;
using Shared.ValidationModels;

namespace Portal.Api;

public static class Extensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCustomOpenTelemetry()
            .AddMediatR()
            .AddCorrelationId()
            .AddHealthChecks(builder.Configuration)
            .AddGrpc(builder.Configuration)
            .AddGraphQl()
            .AddCustomMassTransit(builder.Configuration)
            .AddCustomDbContext(builder.Configuration, builder.Environment)
            .AddAuthentication(builder.Configuration)
            .AddCustomStorage(builder.Configuration)
            .AddSecurityContext()
            .AddCustomSerializer<NewtonSoftService>();

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<Anchor>()
            .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

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
                    endpoints.MapGrpcService<PortalService>();
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

                hcBuilder.AddRabbitMQ(name: "portal-rabbitmqbus-check", tags: new[] { "rabbitmqbus" });
                break;
        }

        var connString = configuration.GetConnectionString("portal");

        if (connString is not null)
        {
            hcBuilder.AddNpgSql(connString, name: "portal-check", tags: new[] { "portalDb" });
        }

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Anchor).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ActivityLogBehavior<,>));

        return services;
    }

    private static IServiceCollection AddGrpc(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        services.AddTransient<ClientLoggerInterceptor>();

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
            options.EnableDetailedErrors = true;
        });

        var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

        services
            .AddGrpcClient<MasterDataApi.MasterDataApiClient>(o => { o.Address = new Uri(serviceOptions.MasterDataService.GrpcUrl); })
            .AddInterceptor<ClientLoggerInterceptor>();

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
                    x.UsingRabbitMq((_, cfg) =>
                    {
                        cfg.Host(new Uri(messageBusOptions.RabbitMq.Url), "/", hc =>
                        {
                            hc.Username(messageBusOptions.RabbitMq.UserName);
                            hc.Password(messageBusOptions.RabbitMq.Password);
                        });

                        ConfigureEndpoint(cfg);
                    });
                    break;
                case MessageBusTransportType.Memory:
                    x.UsingInMemory((_, cfg) =>
                    {
                        cfg.ConcurrentMessageLimit = messageBusOptions.Memory.TransportConcurrencyLimit;

                        ConfigureEndpoint(cfg);
                    });
                    break;
                default:
                    throw new NotSupportedException();
            }
        });

        return services;

        void ConfigureEndpoint(IBusFactoryConfigurator cfg)
        {
            var correlationContextAccessor = services.BuildServiceProvider().GetRequiredService<ICorrelationContextAccessor>();

            cfg.ConfigureSend(x => { x.UseCorrelationId(correlationContextAccessor); });

            cfg.UseCorrelationId(correlationContextAccessor);

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource("PortalService")
                .ConfigureResource(resource =>
                    resource.AddService(
                        "PortalService",
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
        var connString = configuration.GetConnectionString("portal");

        services.AddDbContext<PortalContext>(options =>
        {
            options.UseNpgsql(connString, opt => { opt.EnableRetryOnFailure(3); });

            options.EnableDetailedErrors();

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<DbContext>(provider => provider.GetService<PortalContext>()!);

        services.AddAuditLogs<PortalContext>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");

        services.AddAuthorization(o =>
        {
            o.AddPolicy(AuthorizationPolicy.ADMIN_ACCESS,
                policy =>
                {
                    policy.RequireAssertion(c => c.User.IsInRole(Roles.ADMIN_ROLE_NAME)
                                                 || c.User.IsInRole(Roles.SUPER_USER_ROLE_NAME));
                });

            o.AddPolicy(AuthorizationPolicy.MEMBER_ACCESS,
                policy => { policy.RequireAssertion(c => c.User.IsInRole(Roles.MEMBER_ROLE_NAME)); });

            o.AddPolicy(AuthorizationPolicy.ADMIN_MEMBER_ACCESS,
                policy =>
                {
                    policy.RequireAssertion(c => c.User.IsInRole(Roles.ADMIN_ROLE_NAME)
                                                 || c.User.IsInRole(Roles.SUPER_USER_ROLE_NAME)
                                                 || c.User.IsInRole(Roles.MEMBER_ROLE_NAME));
                });
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = serviceOptions.IdentityService.Url;
                options.RequireHttpsMetadata = false;
                options.Audience = "portal";

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

    private static IServiceCollection AddCustomStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(configuration.GetSection("Storage"));

        services
            .AddStorage()
            .AddFileSystemStorage();

        return services;
    }
}