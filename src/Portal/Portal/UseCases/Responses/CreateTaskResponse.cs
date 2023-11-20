using Shared.Common.ApiResponse;

namespace Portal.UseCases.Responses;

public class CreateTaskResponse : BaseApiResponse
{
    public string? TaskId { get; set; }
    public string? ProjectId { get; set; }
}