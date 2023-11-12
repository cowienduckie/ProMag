using Communication.Boundaries.Grpc;
using Communication.EmailTemplates;
using Communication.EmailTemplates.Implementations;
using Communication.IntegrationEvents.Consumers;
using Configuration.MassTransit;
using Configuration.Metrics;
using Configuration.OpenTelemetry;
using Configuration.OpenTelemetry.Behaviors;
using Email.MailKit;
using HealthChecks.UI.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Shared;
using Shared.Common.Constants;
using Shared.CorrelationId;
using Shared.CustomTypes;
using Shared.Grpc;

namespace Communication.Api;

public static class Extensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCustomOpenTelemetry()
            .AddAppMetrics()
            .AddMediatR()
            .AddCorrelationId()
            .AddHealthChecks(builder.Configuration)
            .AddGrpc()
            .AddCustomMassTransit(builder.Configuration)
            .AddMailKit(builder.Configuration)
            .AddLazyCache();

        builder.Services.AddSingleton<IHandlebarsRender, HandlebarsRender>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCorrelationId()
            .UseAppMetrics()
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<CommunicationService>();

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
            });

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

                hcBuilder.AddRabbitMQ(name: "communication-rabbitmqbus-check", tags: new[] { "rabbitmqbus" });
                break;
        }

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Anchor).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));

        return services;
    }

    private static IServiceCollection AddGrpc(this IServiceCollection services)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

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
            cfg.ReceiveEndpoint(QueueName.Communication, x =>
            {
                x.Consumer<AccountUnlockedEmailConsumer>(ctx);
                x.Consumer<ActivationEmailConsumer>(ctx);
                x.Consumer<ResetPasswordEmailConsumer>(ctx);
            });

            var correlationContextAccessor = services.BuildServiceProvider().GetRequiredService<ICorrelationContextAccessor>();

            cfg.ConfigureSend(x => { x.UseCorrelationId(correlationContextAccessor); });

            cfg.UseCorrelationId(correlationContextAccessor);

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource("CommunicationService")
                .ConfigureResource(resource =>
                    resource.AddService(
                        "CommunicationService",
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
}