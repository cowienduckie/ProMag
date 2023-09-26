using System.Net;
using Communication.Api;
using Configuration.Vault;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Logging;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseLogging()
    .UseVault()
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
        options.Listen(IPAddress.Any, 5002);
        options.Listen(IPAddress.Any, 15002, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
    })
    .UseKestrel(options => { options.AllowSynchronousIO = true; });

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.Run();