using Configuration.MassTransit.CorrelationId;
using Configuration.MassTransit.Tracing.Consuming;
using Configuration.MassTransit.Tracing.Publishing;
using Configuration.MassTransit.Tracing.Sending;
using MassTransit;
using OpenTelemetry.Trace;
using Shared.CorrelationId;

namespace Configuration.MassTransit;

public static class Extensions
{
    public static void UseOpenTelemetryOnPublish(this IPublishPipelineConfigurator configurator, TracerProvider tracerFactory)
    {
        configurator.ConfigurePublish(
            x => x.ConnectPublishPipeSpecificationObserver(new OpenTelemetryPublishPipeSpecificationObserver()));
    }

    public static void UseOpenTelemetryOnSend(this ISendPipelineConfigurator configurator, TracerProvider tracerFactory)
    {
        configurator.ConfigureSend(x => x.ConnectSendPipeSpecificationObserver(new OpenTelemetrySendPipeSpecificationObserver()));
    }

    public static OpenTelemetryConsumeConfigurationObserver UseOpenTelemetryOnConsume(
        this IConsumePipeConfigurator configurator,
        TracerProvider tracerFactory)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        return new OpenTelemetryConsumeConfigurationObserver(configurator, tracerFactory);
    }

    public static void UseCorrelationId<T>(this IPipeConfigurator<T> configurator, ICorrelationContextAccessor correlationContextAccessor)
        where T : class, PipeContext
    {
        configurator.AddPipeSpecification(new CorrelationLoggerSpecification<T>(correlationContextAccessor));
    }

    public static string GetExchangeName(this Uri value)
    {
        var exchange = value.LocalPath;
        var messageType = exchange.Substring(exchange.LastIndexOf('/') + 1);
        return messageType;
    }
}