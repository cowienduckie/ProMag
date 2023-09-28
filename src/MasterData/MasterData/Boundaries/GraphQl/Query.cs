using MediatR;

namespace MasterData.Boundaries.GraphQl;

public class Query
{
    private readonly IMediator _mediator;

    public Query(IMediator mediator)
    {
        _mediator = mediator;
    }
}