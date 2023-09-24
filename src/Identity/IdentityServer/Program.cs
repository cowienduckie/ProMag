using System.Net;
using IdentityServer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Shared.Logging;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("IdentityServer - Start");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseLogging()
        .ConfigureAppConfiguration((_, config) =>
        {
            config.AddJsonFile("appsettings.json", true);
            config.AddEnvironmentVariables();
            config.AddCommandLine(args);
        });

    builder.WebHost
        .ConfigureKestrel((ctx, options) =>
        {
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            options.Limits.MinRequestBodyDataRate = null;
            options.Listen(IPAddress.Any, 5101);
            options.Listen(IPAddress.Any, 15101, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
        })
        .UseKestrel(options => { options.AllowSynchronousIO = true; });

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "IdentityServer - Unhandled exception");
}
finally
{
    Log.Information("IdentityServer - Shut down completed!");
    Log.CloseAndFlush();
}