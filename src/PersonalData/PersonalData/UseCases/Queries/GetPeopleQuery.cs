using MediatR;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Common.Enums;

namespace PersonalData.UseCases.Queries;

public class GetPeopleQuery : IRequest<IQueryable<PersonDto>>
{
    public GetPeopleQuery(UserType userType)
    {
        UserType = userType;
    }

    public UserType UserType { get; }
}