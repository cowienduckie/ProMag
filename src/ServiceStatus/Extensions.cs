namespace ServiceStatus;

public static class Extensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddHealthChecksUI()
            .AddInMemoryStorage();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app
            .UseRouting()
            .UseEndpoints(config => { config.MapHealthChecksUI(); });

        return app;
    }
}