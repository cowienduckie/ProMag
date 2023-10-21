using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Logging;
using Portal.Api;
using Portal.Data;
using Shared.Logging;

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
        options.Listen(IPAddress.Any, 5001);
        options.Listen(IPAddress.Any, 15001, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
    })
    .UseKestrel(options => { options.AllowSynchronousIO = true; });

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

DbInitializer.MigrateDatabase(app);

app.Run();