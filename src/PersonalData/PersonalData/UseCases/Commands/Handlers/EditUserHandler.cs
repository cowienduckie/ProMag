using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Common.Converters;
using PersonalData.Data;
using Promag.Protobuf.Identity.V1;
using Shared.ValidationModels;

namespace PersonalData.UseCases.Commands.Handlers;

public class EditUserHandler : IRequestHandler<EditUserCommand, PersonDto>
{
    private readonly PersonalContext _dbContext;
    private readonly IdentityApi.IdentityApiClient _identityApiClient;
    private readonly ILogger<EditUserHandler> _logger;

    public EditUserHandler(
        PersonalContext dbContext,
        IdentityApi.IdentityApiClient identityApiClient,
        ILogger<EditUserHandler> logger)
    {
        _dbContext = dbContext;
        _identityApiClient = identityApiClient;
        _logger = logger;
    }

    public async Task<PersonDto> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(EditUserHandler));

        var originPerson = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.PersonId), cancellationToken);

        if (originPerson is null)
        {
            _logger.LogError("{Handler} - PersonId={PersonId} - Not Found", nameof(EditUserHandler), request.PersonId);

            throw new ValidationException(
                new ValidationResultModel(
                    new List<ValidationError>
                    {
                        new("User is not found", "personId", ErrorCodes.DATA_NOTFOUND)
                    }
                )
            );
        }

        _logger.LogInformation("{Handler} - Save changes to DB", nameof(EditUserHandler));

        var updatedPerson = request.ToPerson(originPerson);

        _dbContext.People.Update(updatedPerson);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - Send gRPC request to Identity Server", nameof(EditUserHandler));

        var updateRequest = new UpdateRolesRequest
        {
            UserId = request.PersonId
        };
        updateRequest.RoleIds.AddRange(request.RoleIds);

        await _identityApiClient.UpdateUserRolesAsync(updateRequest, cancellationToken: cancellationToken);

        _logger.LogInformation("{Handler} - Finish", nameof(EditUserHandler));

        return updatedPerson.ToPersonDto();
    }
}