using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Common;
using PersonalData.Data;
using Promag.Protobuf.Identity.V1;
using Shared.Common.Enums;

namespace PersonalData.UseCases.Commands.Handlers;

public class LockUserHandler : IRequestHandler<LockUserCommand, bool>
{
    private readonly PersonalContext _context;
    private readonly IdentityApi.IdentityApiClient _identityApiClient;
    private readonly ILogger<LockUserHandler> _logger;

    public LockUserHandler(
        IdentityApi.IdentityApiClient identityApiClient,
        ILogger<LockUserHandler> logger,
        PersonalContext context)
    {
        _identityApiClient = identityApiClient;
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(LockUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(LockUserHandler));
        _logger.LogInformation("{Handler} - Send gRPC request to Identity Server", nameof(LockUserHandler));

        var result = _identityApiClient.LockUserAccount(new LockUserRequest
        {
            UserId = request.PersonId.ToString()
        }, cancellationToken: cancellationToken);

        if (!result.Success)
        {
            _logger.LogError("{Handler} - Unexpected error(s) occured from Identity Server", nameof(LockUserHandler));

            throw result.Errors.ToValidationException();
        }

        _logger.LogInformation("{Handler} - Update Person status to DB", nameof(LockUserHandler));

        var person = await _context.People.FirstOrDefaultAsync(p => p.Id == request.PersonId, cancellationToken);

        if (person is null)
        {
            _logger.LogError("{Handler} - Requested Person ID not found", nameof(LockUserHandler));

            return false;
        }

        person.UserStatus = UserStatus.Lock;

        _context.People.Update(person);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - Finish", nameof(LockUserHandler));

        return true;
    }
}