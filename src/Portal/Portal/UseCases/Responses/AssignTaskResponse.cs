using Shared.Common.ApiResponse;

namespace Portal.UseCases.Responses;

public class AssignTaskResponse : BaseApiResponse
{
    public string? ProjectId { get; set; }
}