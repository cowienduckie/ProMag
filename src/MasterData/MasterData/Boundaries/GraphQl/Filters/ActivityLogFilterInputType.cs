using GraphQl.CustomFilters;
using HotChocolate.Data.Filters;
using MasterData.Boundaries.GraphQl.Dtos;

namespace MasterData.Boundaries.GraphQl.Filters;

public class ActivityLogFilterInputType : FilterInputType<ActivityLogDto>
{
    protected override void Configure(IFilterInputTypeDescriptor<ActivityLogDto> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(t => t.Username).Type<EqualAndContainStringFilter>();
        descriptor.Field(t => t.Action).Type<EqualStringFilter>();
    }
}