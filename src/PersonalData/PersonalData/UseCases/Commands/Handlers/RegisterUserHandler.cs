using Configuration.MassTransit.IntegrationEvents.Email;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalData.Common;
using PersonalData.Common.Constants;
using PersonalData.Common.Converters;
using PersonalData.Data;
using PersonalData.Domain;
using PersonalData.Services;
using PersonalData.UseCases.Responses;
using Promag.Protobuf.Identity.V1;
using Shared.CorrelationId;
using Shared.CustomTypes;
using Shared.ValidationModels;

namespace PersonalData.UseCases.Commands.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IBus _bus;
    private readonly PersonalContext _context;
    private readonly ICorrelationContextAccessor _correlationContext;
    private readonly IdentityApi.IdentityApiClient _identityApiClient;
    private readonly IIdentityService _identityService;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        IBus bus,
        PersonalContext context,
        ILogger<RegisterUserHandler> logger,
        ICorrelationContextAccessor correlationContext,
        IdentityApi.IdentityApiClient identityApiClient,
        IIdentityService identityService)
    {
        _bus = bus;
        _logger = logger;
        _context = context;
        _identityApiClient = identityApiClient;
        _identityService = identityService;
        _correlationContext = correlationContext;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(RegisterUserHandler));

        var isEmailTaken = _context.People.Any(p => p.Email.ToLower() == request.Email.ToLower());

        if (isEmailTaken)
        {
            _logger.LogError("{Handler} - Email={Email} is already taken", nameof(RegisterUserHandler), request.Email);

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

        _logger.LogInformation("{Handler} - Send request to Identity Server", nameof(RegisterUserHandler));

        var roles = await _identityService.FetchAllRoles(cancellationToken);
        var memberRole = roles.FirstOrDefault(x => x.Name == Roles.MEMBER_ROLE_NAME);

        if (memberRole is null)
        {
            _logger.LogError("{Handler} - Member role is not found", nameof(RegisterUserHandler));

            throw new ValidationException(
                new ValidationResultModel(
                    new List<ValidationError>
                    {
                        new("Member role is not found.", "role", ErrorCodes.DATA_NOTFOUND)
                    }
                )
            );
        }

        var createLogInUserRequest = new CreateLogInUserRequest
        {
            UserId = person.Id.ToString(),
            Email = person.Email,
            UserName = request.UserName
        };

        createLogInUserRequest.RoleIds.Add(memberRole.RoleId);

        var createLoginAccountResult = await _identityApiClient.CreateLoginAccountAsync(createLogInUserRequest, cancellationToken: cancellationToken);

        if (!createLoginAccountResult.Succeeded)
        {
            _logger.LogError("{Handler} - Unexpected error(s) from Identity Server", nameof(RegisterUserHandler));

            throw createLoginAccountResult.Errors.ToValidationException();
        }

        _logger.LogInformation("{Handler} - Save new person to DB", nameof(RegisterUserHandler));

        person.ActorId = person.Id;

        await _context.People.AddAsync(person, cancellationToken);

        _logger.LogInformation("{Handler} - Save new workspace and teams to DB", nameof(RegisterUserHandler));

        await _context.Workspaces.AddAsync(new Workspace
        {
            Name = DefaultNameConstants.Workspace,
            Members = new List<Person> { person },
            Teams = new List<Team>
            {
                new()
                {
                    Name = DefaultNameConstants.Team,
                    Members = new List<Person> { person }
                }
            }
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - Publish event by Email from Communication service", nameof(RegisterUserHandler));

        await _bus.Send(new SendActiveAccountEmail
        (
            _correlationContext.CorrelationContext?.CorrelationId ?? Guid.Empty,
            person.Email,
            $"{person.FirstName} {person.LastName}",
            createLoginAccountResult.UserName,
            createLoginAccountResult.ActivateUrl
        ), cancellationToken);

        _logger.LogInformation("{Handler} - Finish", nameof(RegisterUserHandler));

        return new RegisterUserResponse
        {
            UserId = person.Id.ToString()
        };
    }
}