using MediatR;
using PersonalData.Boundaries.GraphQl.Dtos;

namespace PersonalData.UseCases.Queries;

public class GetPersonByIdQuery : IRequest<PersonDto?>
{
    public GetPersonByIdQuery(Guid personId)
    {
        PersonId = personId;
    }

    public Guid PersonId { get; set; }
}