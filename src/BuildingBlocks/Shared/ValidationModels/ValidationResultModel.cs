using System.Net;
using System.Text.Json;

namespace Shared.ValidationModels;

public class ValidationResultModel
{
    public ValidationResultModel(List<ValidationError> errors)
    {
        Errors = errors;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; } = "Validation Failed";
    public List<ValidationError> Errors { get; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}