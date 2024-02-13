using Bogus;
using Fusion.Core.Auth;
using Fusion.Tests.Helpers;

namespace Fusion.Tests.Core.Auth.Helpers;

public class UserRoleDtoFakeProvider() : ObjectFakeProviderBase<UserRoleDto>(5)
{
    protected override Func<Faker<UserRoleDto>> DefaultFactory =>
        () => new Faker<UserRoleDto>()
            .RuleFor(x => x.FusionUserId, f => f.Random.Int())
            .RuleFor(x => x.Roles, f => f.Random.ArrayElements(AuthDefaults.Roles.All.Keys.ToArray(), 2).ToList());
}