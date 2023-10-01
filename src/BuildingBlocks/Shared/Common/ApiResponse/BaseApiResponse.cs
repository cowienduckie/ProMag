namespace Shared.Common.ApiResponse;

public abstract class BaseApiResponse
{
    public int StatusCode { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}