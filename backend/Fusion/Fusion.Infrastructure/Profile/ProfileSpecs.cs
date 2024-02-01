using System.Linq.Expressions;
using Fusion.Infrastructure.Database.Specifications;

namespace Fusion.Infrastructure.Profile;

public static class ProfileSpecs
{
    public static ByFirstNameSpecification ByFirstName(string name) => new(name);
    public static ByLastNameSpecification ByLastName(string name) => new(name);
}

public class ByFirstNameSpecification(string firstName) : Specification<ProfileEntity>
{
    public override Expression<Func<ProfileEntity, bool>> ToExpression()
        => x => x.FirstName.Equals(firstName);
}

public class ByLastNameSpecification(string lastName) : Specification<ProfileEntity>
{
    public override Expression<Func<ProfileEntity, bool>> ToExpression()
        => x => x.LastName.Equals(lastName);
}