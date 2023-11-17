using MediatR;

// ReSharper disable once CheckNamespace
namespace Promag.Protobuf.Identity.V1;

public partial class CreateLogInUserRequest : IRequest<CreateLogInUserResponse>;

public partial class GetUserRolesByUserIdsRequest : IRequest<List<UserRoleDto>>;

public partial class GetRoleByIdsRequest : IRequest<GetRolesResponse>;

public partial class UpdateRolesRequest : IRequest<UpdateRolesResponse>;

public partial class AccountRequest : IRequest<AccountResponse>;

public partial class LockUserRequest : IRequest<LockUserResponse>;

public partial class RoleClaimsRequest : IRequest<RoleClaimsResponse>;

public partial class UpdateRoleClaimsRequest : IRequest<UpdateRoleClaimsResponse>;