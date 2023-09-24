using MassTransit.Configuration;

namespace Configuration.MassTransit.Tracing.Sending;

public class OpenTelemetrySendPipeSpecificationObserver : ISendPipeSpecificationObserver
{
    public void MessageSpecificationCreated<T>(IMessageSendPipeSpecification<T> specification) where T : class
    {
        specification.AddPipeSpecification(new OpenTelemetrySendSpecification<T>());
    }
}