using HotChocolate.Data.Filters;
using Portal.Boundaries.GraphQL.Dtos.Projects;

namespace Portal.Boundaries.GraphQL.Filters;

public class SimplifiedProjectFilter : FilterInputType<SimplifiedProjectDto>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<SimplifiedProjectDto> descriptor)
    {
        descriptor.BindFieldsExplicitly();
    }
}