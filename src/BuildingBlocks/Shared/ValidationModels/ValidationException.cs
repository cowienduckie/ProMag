using Shared.CustomTypes;

namespace Shared.ValidationModels;

public class ValidationException : CustomException
{
    public ValidationException(ValidationResultModel validationResultModel)
    {
        ValidationResultModel = validationResultModel;
    }

    public ValidationResultModel ValidationResultModel { get; private set; }
}