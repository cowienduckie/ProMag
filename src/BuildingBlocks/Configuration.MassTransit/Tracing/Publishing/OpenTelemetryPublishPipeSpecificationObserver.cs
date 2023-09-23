using MassTransit.Configuration;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Publishing;

public class OpenTelemetryPublishPipeSpecificationObserver : IPublishPipeSpecificationObserver
{
    private readonly TracerProvider _tracerProvider;

    public OpenTelemetryPublishPipeSpecificationObserver(TracerProvider tracerProvider)
    {
        _tracerProvider = tracerProvider;
    }

    void IPublishPipeSpecificationObserver.MessageSpecificationCreated<T>(IMessagePublishPipeSpecification<T> specification)
    {
        var openTelemetryPublishSpecification = new OpenTelemetryPublishSpecification<T>(_tracerProvider);

        specification.AddPipeSpecification(openTelemetryPublishSpecification);
    }
}