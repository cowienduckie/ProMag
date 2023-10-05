using System.Diagnostics;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalData.Common;
using Promag.Protobuf.MasterData.V1;
using Shared;
using Shared.CustomTypes;
using Shared.SecurityContext;
using Shared.Serialization;

namespace PersonalData.Data.Audit;

public class ActivityLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ActivityLogBehavior<TRequest, TResponse>> _logger;
    private readonly MasterDataApi.MasterDataApiClient _masterDataApiClient;
    private readonly ISecurityContextAccessor _securityContext;
    private readonly ISerializerService _serializer;

    public ActivityLogBehavior(
        ILogger<ActivityLogBehavior<TRequest, TResponse>> logger,
        MasterDataApi.MasterDataApiClient masterDataApiClient,
        ISecurityContextAccessor securityContext,
        IConfiguration configuration, ISerializerService serializer)
    {
        _logger = logger;
        _masterDataApiClient = masterDataApiClient;
        _securityContext = securityContext;
        _configuration = configuration;
        _serializer = serializer;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (typeof(TRequest).GetCustomAttribute<ActivityLogAttribute>() is null)
        {
            return await next();
        }

        var watch = Stopwatch.StartNew();
        var response = await next();
        watch.Stop();

        var appOptions = _configuration.GetOptions<AppOptions>("App");

        var createActivityLogRequest = new CreateActivityLogRequest
        {
            IpAddress = _securityContext.IpAddressClient,
            Service = appOptions.Name,
            Action = request.GetGenericTypeName(),
            Duration = watch.ElapsedMilliseconds,
            Parameters = _serializer.Serialize(request),
            Username = _securityContext.Username
        };

        var createLogResponse =
            await _masterDataApiClient.CreateActivityLogAsync(createActivityLogRequest, cancellationToken: cancellationToken);

        if (createLogResponse.Succeeded)
        {
            _logger.LogError("ActivityLogBehavior - RequestType={Request} - ResponseType={Response} - Succeeded",
                typeof(TRequest), typeof(TResponse));

            return response;
        }

        var error = createLogResponse.Errors.ToValidationException();

        _logger.LogError("ActivityLogBehavior - RequestType={Request} - ResponseType={Response} - Ex={Message}",
            typeof(TRequest), typeof(TResponse), error.Message);

        return response;
    }
}