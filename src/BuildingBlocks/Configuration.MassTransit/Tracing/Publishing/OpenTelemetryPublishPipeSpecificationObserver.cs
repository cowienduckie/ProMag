using MassTransit.Configuration;

namespace Configuration.MassTransit.Tracing.Publishing;

public class OpenTelemetryPublishPipeSpecificationObserver : IPublishPipeSpecificationObserver
{
    void IPublishPipeSpecificationObserver.MessageSpecificationCreated<T>(IMessagePublishPipeSpecification<T> specification)
    {
        var openTelemetryPublishSpecification = new OpenTelemetryPublishSpecification<T>();

        specification.AddPipeSpecification(openTelemetryPublishSpecification);
    }
}