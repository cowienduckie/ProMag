using HotChocolate.Types;
using MasterData.Boundaries.GraphQl.Dtos;

namespace MasterData.Boundaries.GraphQl.ObjectTypes;

public class ActivityLogType : ObjectType<ActivityLogDto>
{
    protected override void Configure(IObjectTypeDescriptor<ActivityLogDto> descriptor)
    {
        descriptor.Field(x => x.CreatedOn).Type<NonNullType<DateTimeType>>();
    }
}