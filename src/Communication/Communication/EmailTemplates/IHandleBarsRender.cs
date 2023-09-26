namespace Communication.EmailTemplates;

public interface IHandlebarsRender
{
    Task<string?> RenderAsync<TModel>(string templateName, TModel model, bool htmlFormat = true, CancellationToken cancellationToken = default);
}