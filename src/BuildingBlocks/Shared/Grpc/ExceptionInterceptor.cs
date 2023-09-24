using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Shared.ValidationModels;

namespace Shared.Grpc;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ExceptionInterceptor>();
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "{ExMessage}", ex.ValidationResultModel.ToString());

            throw new RpcException(new Status(StatusCode.Internal, ex.ValidationResultModel.ToString()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{ExMessage}", ex.Message);

            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}