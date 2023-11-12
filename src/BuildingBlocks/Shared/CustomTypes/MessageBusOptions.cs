using System.Diagnostics.CodeAnalysis;

namespace Shared.CustomTypes;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MessageBusTransportType
{
    public const string RabbitMQ = nameof(RabbitMQ);
    public const string Memory = "Memory";
}

public class MessageBusOptions
{
    public string TransportType { get; set; } = string.Empty;
    public RabbitMqOptions RabbitMq { get; set; } = default!;
    public MemoryOptions Memory { get; set; } = default!;
}

public class RabbitMqOptions
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class MemoryOptions
{
    public int TransportConcurrencyLimit { get; set; }
}