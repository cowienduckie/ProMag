using HotChocolate.Data.Filters;

namespace GraphQl.CustomFilters;

public class EqualStringFilter : StringOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Operation(DefaultFilterOperations.Equals).Type<NonNullType<StringType>>();
    }
}