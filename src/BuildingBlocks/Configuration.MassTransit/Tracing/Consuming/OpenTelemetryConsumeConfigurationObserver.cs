using MassTransit;
using MassTransit.Configuration;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing;

public class OpenTelemetryConsumeConfigurationObserver : ConfigurationObserver, IMessageConfigurationObserver
{
    private readonly TracerProvider _tracerProvider;

    public OpenTelemetryConsumeConfigurationObserver(IConsumePipeConfigurator configurator, TracerProvider tracerProvider) : base(configurator)
    {
        _tracerProvider = tracerProvider;

        Connect(this);
    }

    public void MessageConfigured<T>(IConsumePipeConfigurator configurator) where T : class
    {
        var specification = new OpenTelemetryConsumeSpecification<T>(_tracerProvider);

        configurator.AddPipeSpecification((IPipeSpecification<ConsumeContext>)specification);
    }
}