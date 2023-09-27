using MasterData.Boundaries.GraphQl.Dtos;
using MediatR;

namespace MasterData.UseCases.Queries;

public class GetActivityLogByIdQuery : IRequest<ActivityLogDto>
{
    public GetActivityLogByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}