using System.Linq.Expressions;
using Fusion.Infrastructure.Database.Specifications;

namespace Fusion.Infrastructure.Profile;

public static class ProfileSpecs
{
    public static ByEmailSpecification ByEmail(string email) => new(email);
    public static ByAuth0UserIdSpecification ByAuth0UserId(string auth0UserId) => new(auth0UserId);
}

public class ByEmailSpecification(string email) : Specification<ProfileEntity>
{
    public override Expression<Func<ProfileEntity, bool>> ToExpression()
        => x => x.Email.Equals(email);
}

public class ByAuth0UserIdSpecification(string auth0UserId) : Specification<ProfileEntity>
{
    public override Expression<Func<ProfileEntity, bool>> ToExpression()
        => x => x.Auth0UserId != null && x.Auth0UserId.Equals(auth0UserId);
}