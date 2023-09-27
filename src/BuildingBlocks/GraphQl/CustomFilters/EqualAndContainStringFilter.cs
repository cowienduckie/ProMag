using HotChocolate.Data.Filters;

namespace GraphQl.CustomFilters;

public class EqualAndContainStringFilter : StringOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Operation(DefaultFilterOperations.Equals).Type<NonNullType<StringType>>();
        descriptor.Operation(DefaultFilterOperations.Contains).Type<NonNullType<StringType>>();
    }
}