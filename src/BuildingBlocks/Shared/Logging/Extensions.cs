using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Settings.Configuration;
using Shared.CustomTypes;
using Shared.Logging.Enrichers;

namespace Shared.Logging;

public static class Extensions
{
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, string applicationName = "")
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            var appOptions = context.Configuration.GetOptions<AppOptions>("App");
            var loggingOptions = context.Configuration.GetOptions<LoggingOptions>("Logging");

            applicationName = string.IsNullOrWhiteSpace(applicationName) ? appOptions.Name : applicationName;

            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration, new ConfigurationReaderOptions { SectionName = "Logging" })
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.With<OpenTracingContextLogEventEnricher>();

            Configure(loggerConfiguration, loggingOptions.Seq, loggingOptions);
        });

        return hostBuilder;
    }

    private static void Configure(LoggerConfiguration loggerConfiguration, SeqOptions seq, LoggingOptions loggingOptions)
    {
        if (seq.Enabled)
        {
            loggerConfiguration.WriteTo.Seq(seq.Url, apiKey: seq.ApiKey);
        }

        if (loggingOptions.ConsoleEnabled)
        {
            loggerConfiguration.WriteTo
                .Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {Properties:j}] {Message:lj}{NewLine}{Exception}");
        }
    }
}