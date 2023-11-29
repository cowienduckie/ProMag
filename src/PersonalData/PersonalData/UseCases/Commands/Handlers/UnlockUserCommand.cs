using Configuration.MassTransit.IntegrationEvents.Email;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Common;
using PersonalData.Data;
using Promag.Protobuf.Identity.V1;
using Shared.Common.Enums;
using Shared.CorrelationId;

namespace PersonalData.UseCases.Commands.Handlers;

public class UnlockUserHandler : IRequestHandler<UnlockUserCommand, bool>
{
    private readonly IBus _bus;
    private readonly PersonalContext _context;
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly IdentityApi.IdentityApiClient _identityApiClient;
    private readonly ILogger<UnlockUserHandler> _logger;

    public UnlockUserHandler(PersonalContext context,
        IdentityApi.IdentityApiClient identityApiClient,
        ICorrelationContextAccessor correlationContextAccessor,
        ILogger<UnlockUserHandler> logger,
        IBus bus)
    {
        _identityApiClient = identityApiClient;
        _bus = bus;
        _correlationContextAccessor = correlationContextAccessor;
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(UnlockUserHandler));
        _logger.LogInformation("{Handler} - Send gRPC request to Identity Server", nameof(UnlockUserHandler));

        var result = _identityApiClient.UnlockUserAccount(new AccountRequest
        {
            UserId = request.PersonId
        }, cancellationToken: cancellationToken);

        if (!result.Success)
        {
            _logger.LogError("{Handler} - Unexpected error(s) occured from Identity Server", nameof(UnlockUserHandler));

            throw result.Errors.ToValidationException();
        }


        _logger.LogInformation("{Handler} - Update Person status to DB", nameof(UnlockUserHandler));

        var person = await _context.People.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.PersonId), cancellationToken);

        if (person is null)
        {
            _logger.LogError("{Handler} - PersonId={PersonId} not found", nameof(UnlockUserHandler), request.PersonId);

            return false;
        }

        person.UserStatus = UserStatus.Active;

        _context.People.Update(person);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - Send Email request to Community service", nameof(UnlockUserHandler));

        var personFullName = $"{person.FirstName} {person.LastName}";

        await _bus.Send(new SendAccountUnlockedEmail
        (
            _correlationContextAccessor.CorrelationContext?.CorrelationId ?? Guid.Empty,
            person.Email,
            personFullName,
            result.UserName,
            result.ResetPasswordUrl
        ), cancellationToken);

        _logger.LogInformation("{Handler} - Finish", nameof(UnlockUserHandler));

        return true;
    }
}