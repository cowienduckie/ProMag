using MassTransit.Configuration;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Sending;

public class OpenTelemetrySendPipeSpecificationObserver : ISendPipeSpecificationObserver
{
    private readonly TracerProvider _tracerProvider;

    public OpenTelemetrySendPipeSpecificationObserver(TracerProvider tracerProvider)
    {
        _tracerProvider = tracerProvider;
    }

    public void MessageSpecificationCreated<T>(IMessageSendPipeSpecification<T> specification) where T : class
    {
        specification.AddPipeSpecification(new OpenTelemetrySendSpecification<T>(_tracerProvider));
    }
}