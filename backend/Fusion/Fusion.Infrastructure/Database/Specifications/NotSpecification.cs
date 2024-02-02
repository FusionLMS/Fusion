using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Fusion.Infrastructure.Database.Specifications;

[ExcludeFromCodeCoverage]
public class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _specification;

    public NotSpecification(Specification<T> spec)
    {
        ArgumentNullException.ThrowIfNull(spec);
        _specification = spec;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = _specification.ToExpression();
        return Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;

        if (ReferenceEquals(this, obj)) return true;

        if (obj is NotSpecification<T> otherSpec) return _specification.Equals(otherSpec);

        return false;
    }

    public override int GetHashCode()
    {
        return _specification.GetHashCode() ^ GetType().GetHashCode();
    }
    
}