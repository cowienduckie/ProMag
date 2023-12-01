using HandlebarsDotNet;
using LazyCache;

namespace Communication.EmailRender.Implementations;

public class HandlebarsRender : IHandlebarsRender
{
    private readonly IAppCache _lazyCache;
    private readonly SemaphoreSlim _locker = new(1, 1);

    private bool _registeredHelpersAndPartials;

    public HandlebarsRender(IAppCache lazyCache)
    {
        _lazyCache = lazyCache;
    }

    public async Task<string?> RenderAsync<TModel>(
        string templateName,
        TModel model,
        bool htmlFormat = true,
        CancellationToken cancellationToken = default)
    {
        await _locker.WaitAsync(100, cancellationToken);

        try
        {
            await RegisterHelpersAndPartialsAsync(htmlFormat, cancellationToken);
        }
        finally
        {
            _locker.Release();
        }

        var template = await _lazyCache.GetOrAddAsync(templateName, async () =>
        {
            var source = await ReadSourceAsync(templateName, htmlFormat, cancellationToken);

            return Handlebars.Compile(source);
        });

        return template?.Invoke(model);
    }

    private async Task RegisterHelpersAndPartialsAsync(bool htmlFormat, CancellationToken cancellationToken = default)
    {
        if (_registeredHelpersAndPartials)
        {
            return;
        }

        _registeredHelpersAndPartials = true;

        var basicHtmlLayoutSource = await ReadSourceAsync("Layouts/Basic", htmlFormat, cancellationToken);

        Handlebars.RegisterTemplate("BasicHtmlLayout", basicHtmlLayoutSource);
    }

    private static async Task<string> ReadSourceAsync(string templateName, bool htmlFormat, CancellationToken cancellationToken = default)
    {
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "EmailTemplates/Handlebars",
            $"{templateName}.{(htmlFormat ? "html.hbs" : "text.hbs")}");

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException();
        }

        return await File.ReadAllTextAsync(templatePath, cancellationToken);
    }
}