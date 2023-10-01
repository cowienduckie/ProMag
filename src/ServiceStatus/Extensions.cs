namespace ServiceStatus;

public static class Extensions
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddHealthChecksUI()
            .AddInMemoryStorage();

        return builder.Build();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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