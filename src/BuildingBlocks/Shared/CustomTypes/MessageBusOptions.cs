namespace Shared.CustomTypes;

public class MessageBusOptions
{
    public MessageBusOptions(string transportType, RabbitMqOptions rabbitMq, AzureSbOptions azureSb, MemoryOptions memory)
    {
        TransportType = transportType;
        RabbitMq = rabbitMq;
        AzureSb = azureSb;
        Memory = memory;
    }

    public string TransportType { get; set; }
    public RabbitMqOptions RabbitMq { get; set; }
    public AzureSbOptions AzureSb { get; set; }
    public MemoryOptions Memory { get; set; }
}

public class RabbitMqOptions
{
    public RabbitMqOptions(string userName, string password, string url)
    {
        UserName = userName;
        Password = password;
        Url = url;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
    public string Url { get; set; }
}

public class AzureSbOptions
{
    public AzureSbOptions(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; set; }
}

public class MemoryOptions
{
    public MemoryOptions(int transportConcurrencyLimit)
    {
        TransportConcurrencyLimit = transportConcurrencyLimit;
    }

    public int TransportConcurrencyLimit { get; set; }
}