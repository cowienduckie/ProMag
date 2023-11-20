using GraphQl.CustomFilters;
using HotChocolate.Data.Filters;
using PersonalData.Boundaries.GraphQl.Dtos;

namespace PersonalData.Boundaries.GraphQl.Filters;

public class PersonFilterType : FilterInputType<PersonDto>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<PersonDto> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(t => t.FirstName).Type<EqualOrContainStringFilter>();
        descriptor.Field(t => t.LastName).Type<EqualOrContainStringFilter>();
        descriptor.Field(t => t.UserStatus).Type<EqualIntFilter>();
    }
}