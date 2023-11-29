using Configuration.MassTransit;
using Configuration.MassTransit.IntegrationEvents.Account;
using Configuration.MassTransit.IntegrationEvents.Email;
using Configuration.MassTransit.IntegrationEvents.Logging;
using Configuration.Metrics;
using Configuration.OpenTelemetry;
using Configuration.OpenTelemetry.Behaviors;
using HealthChecks.UI.Client;
using IdentityServer.Boundaries.Grpc;
using IdentityServer.Data;
using IdentityServer.Models.DbModel;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Serilog;
using Shared;
using Shared.Common.Enums;
using Shared.Common.Helpers;
using Shared.CorrelationId;
using Shared.CustomTypes;
using Shared.Grpc;
using Shared.ValidationModels;

namespace IdentityServer;

internal static class Extensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        builder.Services
            .AddGrpc()
            .AddCustomMassTransit(builder.Configuration)
            .AddMediatR()
            .AddCorrelationId()
            .AddCustomOpenTelemetry()
            .AddAppMetrics()
            .AddCustomHealthChecks(builder.Configuration)
            .AddCustomDbContext(builder.Configuration, builder.Environment)
            .AddOptions()
            .AddIdentityServer(builder.Configuration, builder.Environment)
            .AddHttpContextAccessor()
            .AddDatabaseDeveloperPageExceptionFilter();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }

        var forwardOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            RequireHeaderSymmetry = false
        };
        forwardOptions.KnownNetworks.Clear();
        forwardOptions.KnownProxies.Clear();

        app.UseForwardedHeaders(forwardOptions)
            .UseCorrelationId()
            .UseAppMetrics()
            .UsePathBase(app.Configuration["PathBase"])
            .UseIdentityServer()
            .UseStaticFiles()
            .UseRouting()
            .UseCookiePolicy()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();

                endpoints.MapGrpcService<IdentityService>();

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
            });

        return app;
    }

    private static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetOptions<AppOptions>("App");

        if (!appOptions.HealthCheckEnabled)
        {
            return services;
        }

        var connString = configuration.GetConnectionString("identity");

        var hcBuilder = services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddNpgSql(
                connString!,
                name: "identityDataDb-check",
                tags: new[] { "identityDataDb" }
            );

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
                hcBuilder.AddRabbitMQ(name: "idsvr-rabbitmqbus-check", tags: new[] { "rabbitmqbus" });
                break;
        }

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IdentityService).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services)
    {
        services.Configure<IISOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });

        return services;
    }

    private static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connString = configuration.GetConnectionString("identity");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connString);
            options.EnableDetailedErrors();

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        return services;
    }

    private static IServiceCollection AddGrpc(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
            options.EnableDetailedErrors = true;
        });

        return services;
    }

    private static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
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
            EndpointConvention.Map<SendResetPasswordEmail>(new Uri($"queue:{QueueName.Communication}"));
            EndpointConvention.Map<AccountStatusChanged>(new Uri($"queue:{QueueName.PersonalData}"));
            EndpointConvention.Map<SaveActivityLog>(new Uri($"queue:{QueueName.MasterData}"));

            var correlationContextAccessor = services.BuildServiceProvider().GetRequiredService<ICorrelationContextAccessor>();

            cfg.ConfigureSend(x => { x.UseCorrelationId(correlationContextAccessor); });

            cfg.UseCorrelationId(correlationContextAccessor);

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource("IdentityServer")
                .ConfigureResource(resource =>
                    resource.AddService(
                        "IdentityServer",
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

    private static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Lockout = configuration.GetOptions<LockoutOptions>("IdentityServiceOptions:Lockout");
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        var connString = configuration.GetConnectionString("identity");
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

        var builder = services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseNpgsql(connString, db => db.MigrationsAssembly(migrationsAssembly));
                options.DefaultSchema = DbSchema.Identity.GetDescription();
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseNpgsql(connString, db => db.MigrationsAssembly(migrationsAssembly));
                options.DefaultSchema = DbSchema.Identity.GetDescription();
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<ApplicationUser>();

        if (environment.IsDevelopment())
        {
            builder.AddDeveloperSigningCredential();
        }

        services
            .AddAuthentication()
            .AddCookie();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        return services;
    }
}