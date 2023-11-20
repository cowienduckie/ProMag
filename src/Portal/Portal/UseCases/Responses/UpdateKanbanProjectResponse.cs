using Shared.Common.ApiResponse;

namespace Portal.UseCases.Responses;

public class UpdateKanbanProjectResponse : BaseApiResponse
{
    public string? ProjectId { get; set; }
}