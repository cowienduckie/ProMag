using GraphQl.CustomFilters;
using HotChocolate.Data.Filters;
using Portal.Boundaries.GraphQL.Dtos;

namespace Portal.Boundaries.GraphQL.Filters;

public class SimplifiedProjectFilter : FilterInputType<SimplifiedProjectDto>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<SimplifiedProjectDto> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(t => t.Id).Type<EqualAndContainStringFilter>();
        descriptor.Field(t => t.Name).Type<EqualAndContainStringFilter>();
    }
}