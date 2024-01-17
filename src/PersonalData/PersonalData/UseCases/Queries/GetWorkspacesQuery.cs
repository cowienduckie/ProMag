using MediatR;
using PersonalData.UseCases.Responses;

namespace PersonalData.UseCases.Queries;

public class GetWorkspacesQuery : IRequest<GetWorkspacesResponse>;