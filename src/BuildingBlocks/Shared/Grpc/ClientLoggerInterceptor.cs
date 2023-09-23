using Grpc.Core;
using Grpc.Core.Interceptors;
using Shared.CorrelationId;

namespace Shared.Grpc;

public class ClientLoggerInterceptor : Interceptor
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor;

    public ClientLoggerInterceptor(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContextAccessor = correlationContextAccessor ?? throw new ArgumentNullException(nameof(correlationContextAccessor));
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        AddCallerMetadata(ref context);

        return continuation(request, context);
    }

    private void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        var headers = context.Options.Headers;

        if (headers == null)
        {
            headers = new Metadata();

            var options = context.Options.WithHeaders(headers);

            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
        }

        headers.Add(
            _correlationContextAccessor.CorrelationContext.Header,
            _correlationContextAccessor.CorrelationContext.CorrelationId.ToString());
    }
}