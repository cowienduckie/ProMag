using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Data;
using Shared.ValidationModels;

namespace PersonalData.UseCases.Commands.Handlers;

public class UpdateUserAvatarHandler : IRequestHandler<UpdateUserAvatarCommand, bool>
{
    private readonly PersonalContext _dbContext;
    private readonly ILogger<UpdateUserAvatarHandler> _logger;

    public UpdateUserAvatarHandler(PersonalContext dbContext, ILogger<UpdateUserAvatarHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(UpdateUserAvatarHandler));

        var originPerson = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == request.PersonId, cancellationToken);

        if (originPerson is null)
        {
            _logger.LogError("{Handler} - User Id not found", nameof(UpdateUserAvatarHandler));

            throw new ValidationException(
                new ValidationResultModel(
                    new List<ValidationError>
                    {
                        new("User is not found", "personId", ErrorCodes.DATA_NOTFOUND)
                    }
                )
            );
        }

        originPerson.PhotoPath = request.AvatarFileName;

        _dbContext.People.Update(originPerson);

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - Finish", nameof(UpdateUserAvatarHandler));

        return result > 0;
    }
}