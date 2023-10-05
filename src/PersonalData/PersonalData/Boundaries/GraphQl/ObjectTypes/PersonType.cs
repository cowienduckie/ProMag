using HotChocolate.Types;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Services;
using UserRoleDto = Promag.Protobuf.Identity.V1.UserRoleDto;

namespace PersonalData.Boundaries.GraphQl.ObjectTypes;

public class PersonType : ObjectType<PersonDto>
{
    protected override void Configure(IObjectTypeDescriptor<PersonDto> descriptor)
    {
        descriptor
            .Field("roles")
            .Type<ListType<UserRoleType>>()
            .Resolve(async ctx =>
            {
                var service = ctx.Service<IIdentityService>();
                var dataLoader = ctx.BatchDataLoader<string, List<UserRoleDto>>(service.FetchUserRolesByPersonIds, "roleByUserId");

                return await dataLoader.LoadAsync(ctx.Parent<PersonDto>().Id);
            });
    }
}