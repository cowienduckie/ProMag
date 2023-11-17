using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Shared.CorrelationId;

[SuppressMessage("Usage", "ASP0019:Suggest using IHeaderDictionary.Append or the indexer")]
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CorrelationIdOptions _options;

    public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task Invoke(HttpContext context, ICorrelationContextFactory correlationContextFactory)
    {
        var correlationId = SetCorrelationId(context);

        if (_options.UpdateTraceIdentifier)
        {
            context.TraceIdentifier = correlationId.ToString();
        }

        correlationContextFactory.Create(correlationId, _options.Header);

        if (_options.IncludeInResponse)
        {
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(_options.Header))
                {
                    context.Response.Headers.Add(_options.Header, correlationId.ToString());
                }

                return Task.CompletedTask;
            });
        }

        using (LogContext.PushProperty(_options.Header, correlationId))
        {
            await _next(context);
        }

        correlationContextFactory.Dispose();
    }

    private Guid SetCorrelationId(HttpContext context)
    {
        var correlationIdFoundInRequestHeader = context.Request.Headers.TryGetValue(_options.Header, out var correlationId);

        if (!RequiresGenerationOfCorrelationId(correlationIdFoundInRequestHeader, correlationId))
        {
            return Guid.Parse(correlationId!);
        }

        correlationId = Guid.NewGuid().ToString();

        if (!context.Request.Headers.ContainsKey(_options.Header))
        {
            context.Request.Headers.Add(_options.Header, correlationId);
        }

        return Guid.Parse(correlationId!);
    }

    private static bool RequiresGenerationOfCorrelationId(bool idInHeader, string? idFromHeader)
    {
        if (!idInHeader || StringValues.IsNullOrEmpty(idFromHeader) || !Guid.TryParse(idFromHeader, out _))
        {
            return true;
        }

        return false;
    }
}