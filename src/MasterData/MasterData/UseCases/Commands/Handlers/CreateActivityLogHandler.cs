using MasterData.Data;
using MasterData.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Promag.Protobuf.MasterData.V1;
using Shared;

namespace MasterData.UseCases.Commands.Handlers;

public class CreateActivityLogHandler : IRequestHandler<CreateActivityLogCommand, CreateActivityLogResponse>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<CreateActivityLogHandler> _logger;

    public CreateActivityLogHandler(ILogger<CreateActivityLogHandler> logger, MasterDataDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<CreateActivityLogResponse> Handle(CreateActivityLogCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;

        Guard.NotNull(payload);

        _logger.LogInformation("{HandlerName} - Start", nameof(CreateActivityLogHandler));

        var activityLog = new ActivityLog
        {
            IpAddress = payload.IpAddress,
            Service = payload.Service,
            Action = payload.Action,
            Duration = payload.Duration,
            Parameters = payload.Parameters,
            Username = payload.Username
        };

        await _context.AddAsync(activityLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{HandlerName} - Finish", nameof(CreateActivityLogHandler));

        return new CreateActivityLogResponse
        {
            Succeeded = true
        };
    }
}