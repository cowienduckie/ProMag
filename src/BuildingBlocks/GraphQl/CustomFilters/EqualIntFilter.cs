using HotChocolate.Data.Filters;

namespace GraphQl.CustomFilters;

public class EqualIntFilter : IntOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Operation(DefaultFilterOperations.Equals).Type<NonNullType<IntType>>();
    }
}