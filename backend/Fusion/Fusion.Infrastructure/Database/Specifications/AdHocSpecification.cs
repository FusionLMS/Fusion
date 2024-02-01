using System.Linq.Expressions;

namespace Fusion.Infrastructure.Database.Specifications;

public class AdHocSpecification<T> : Specification<T>
{
    private readonly Expression<Func<T, bool>> _expression;

    public AdHocSpecification(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _expression = expression;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        return _expression;
    }
}
