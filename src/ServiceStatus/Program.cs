using System.Net;
using ServiceStatus;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.WebHost
    .ConfigureKestrel((_, options) =>
    {
        options.Limits.MinRequestBodyDataRate = null;
        options.Listen(IPAddress.Any, 5104);
    })
    .UseKestrel(options => { options.AllowSynchronousIO = true; });

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.Run();