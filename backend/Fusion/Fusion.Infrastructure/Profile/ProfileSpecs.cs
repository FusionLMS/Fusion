using System.Linq.Expressions;
using Fusion.Infrastructure.Database.Specifications;

namespace Fusion.Infrastructure.Profile;

public static class ProfileSpecs
{
    public static ByEmailSpecification ByEmail(string email) => new(email);
}

public class ByEmailSpecification(string email) : Specification<ProfileEntity>
{
    public override Expression<Func<ProfileEntity, bool>> ToExpression()
        => x => x.Email.Equals(email);
}