using Bogus;
using Fusion.Core.Profile;
using Fusion.Tests.Helpers;

namespace Fusion.Tests.Core.Profile.Helpers;

public class ProfileDtoFakeProvider() : ObjectFakeProviderBase<ProfileDto>(5)
{
    protected override Func<Faker<ProfileDto>> DefaultFactory =>
        () => new Faker<ProfileDto>()
            .RuleFor(x => x.Id, x => x.IndexFaker)
            .RuleFor(x => x.FirstName, x => x.Person.FirstName)
            .RuleFor(x => x.LastName, x => x.Person.LastName)
            .RuleFor(x => x.Email, x => x.Person.Email);
}