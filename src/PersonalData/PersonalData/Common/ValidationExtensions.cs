using Promag.Protobuf.Commons.V1;
using Shared.ValidationModels;

namespace PersonalData.Common;

public static class ValidationExtensions
{
    public static ValidationException ToValidationException(this IEnumerable<ErrorDto> errors)
    {
        var validationErrors = errors
            .Select(err => new ValidationError(err.Description, err.Code))
            .ToList();

        var validationModel = new ValidationResultModel(validationErrors);

        return new ValidationException(validationModel);
    }
}