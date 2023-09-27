using MasterData.Data;
using MasterData.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Promag.Protobuf.MasterData.V1;
using Shared;

namespace MasterData.UseCases.Commands.Handlers;

public class CreateActivityLogHandler : IRequestHandler<CreateActivityLogRequest, CreateActivityLogResponse>
{
    private readonly MasterDataDbContext _context;
    private readonly ILogger<CreateActivityLogHandler> _logger;

    public CreateActivityLogHandler(ILogger<CreateActivityLogHandler> logger, MasterDataDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<CreateActivityLogResponse> Handle(CreateActivityLogRequest request, CancellationToken cancellationToken)
    {
        Guard.NotNull(request);

        _logger.LogInformation("{HandlerName} - Start", nameof(CreateActivityLogHandler));

        var activityLog = new ActivityLog
        {
            Id = Guid.NewGuid(),
            IpAddress = request.IpAddress,
            Service = request.Service,
            Action = request.Action,
            Duration = request.Duration,
            Parameters = request.Parameters,
            Username = request.Username,
            CreatedDate = DateTime.UtcNow
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