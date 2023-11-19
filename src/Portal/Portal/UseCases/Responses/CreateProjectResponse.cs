using Shared.Common.ApiResponse;

namespace Portal.UseCases.Responses;

public class CreateProjectResponse : BaseApiResponse
{
    public string? ProjectId { get; set; }
}