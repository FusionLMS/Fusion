using System.Linq.Expressions;

namespace Fusion.Infrastructure.Database.Specifications;

public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();

        return leftExpr.OrElse(rightExpr);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;

        if (ReferenceEquals(this, obj)) return true;

        if (obj is OrSpecification<T> otherSpec)
            return _left.Equals(otherSpec._left) &&
                   _right.Equals(otherSpec._right);

        return false;
    }

    public override int GetHashCode()
    {
        return _left.GetHashCode() ^ _right.GetHashCode() ^ GetType().GetHashCode();
    }
}