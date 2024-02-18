using Fusion.Infrastructure.Database.Specifications;
using Fusion.Infrastructure.Profile;
using System.Linq.Expressions;

namespace Fusion.Infrastructure.Assignment
{
    public static class AssignmentSpecs
    {
        public static ByTitleSpecification ByTitle(string title) => new(title);
    }

    public class ByTitleSpecification(string title) : Specification<AssignmentEntity>
    {
        public override Expression<Func<AssignmentEntity, bool>> ToExpression()
            => x => x.Title.Equals(title);
    }
}
