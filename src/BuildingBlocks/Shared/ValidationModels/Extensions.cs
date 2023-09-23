using FluentValidation;

namespace Shared.ValidationModels;

public static class Extensions
{
    public static async Task HandleValidation<TRequest>(this IValidator<TRequest> validator, TRequest request, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(err => new ValidationError(err.ErrorMessage, err.PropertyName)).ToList();

            throw new ValidationException(new ValidationResultModel(errors));
        }
    }
}