using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalData.Common;
using PersonalData.Common.Converters;
using PersonalData.Data;
using PersonalData.IntegrationEvents;
using PersonalData.UseCases.Responses;
using Promag.Protobuf.Identity.V1;
using Shared.CorrelationId;
using Shared.ValidationModels;

namespace PersonalData.UseCases.Commands.Handlers;

public class InviteUserHandler : IRequestHandler<InviteUserCommand, InviteUserResponse>
{
    private readonly IBus _bus;
    private readonly PersonalContext _context;
    private readonly ICorrelationContextAccessor _correlationContext;
    private readonly IdentityApi.IdentityApiClient _identityApiClient;
    private readonly ILogger<InviteUserHandler> _logger;

    public InviteUserHandler(
        IBus bus,
        PersonalContext context,
        ILogger<InviteUserHandler> logger,
        ICorrelationContextAccessor correlationContext,
        IdentityApi.IdentityApiClient identityApiClient)
    {
        _bus = bus;
        _logger = logger;
        _context = context;
        _identityApiClient = identityApiClient;
        _correlationContext = correlationContext;
    }

    public async Task<InviteUserResponse> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(InviteUserHandler));

        var isEmailTaken = _context.People.Any(p => string.Equals(p.Email, request.Email, StringComparison.CurrentCultureIgnoreCase));

        if (isEmailTaken)
        {
            _logger.LogError("{Handler} - Email={Email} is already taken", nameof(InviteUserHandler), request.Email);

            throw new ValidationException(
                new ValidationResultModel(
                    new List<ValidationError>
                    {
                        new("Email is already taken.", "email", ErrorCodes.DUPLICATE_EMAIL)
                    }
                )
            );
        }

        var person = request.ToPerson();

        _logger.LogInformation("{Handler} - Send request to Identity Server", nameof(InviteUserHandler));

        var createLogInUserRequest = new CreateLogInUserRequest
        {
            UserId = person.Id.ToString(),
            Email = person.Email,
            UserName = request.UserName
        };
        createLogInUserRequest.RoleIds.AddRange(request.RoleIds);

        var createLoginAccountResult = await _identityApiClient.CreateLoginAccountAsync(createLogInUserRequest, cancellationToken: cancellationToken);

        if (!createLoginAccountResult.Succeeded)
        {
            _logger.LogError("{Handler} - Unexpected error(s) from Identity Server", nameof(InviteUserHandler));

            throw createLoginAccountResult.Errors.ToValidationException();
        }

        _logger.LogInformation("{Handler} - Save new person to DB", nameof(InviteUserHandler));

        person.ActorId = person.Id;

        await _context.People.AddAsync(person, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - Publish event by Email from Communication service", nameof(InviteUserHandler));

        await _bus.Send<ISendActiveAccountEmail>(new // TODO: Move interfaces to Shared then store implementation in each service
        {
            _correlationContext.CorrelationContext?.CorrelationId,
            ReceiverEmail = person.Email,
            createLoginAccountResult.UserName,
            FullName = $"{person.FirstName} {person.LastName}",
            createLoginAccountResult.ActivateUrl
        }, cancellationToken);

        _logger.LogInformation("{Handler} - Finish", nameof(InviteUserHandler));

        return new InviteUserResponse
        {
            PersonId = person.Id.ToString()
        };
    }
}