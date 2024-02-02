using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Fusion.Infrastructure.Database.Specifications;

[ExcludeFromCodeCoverage]
public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return left.Compose(right, Expression.AndAlso);
    }

    public static Expression<Func<T, bool>> OrElse<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return left.Compose(right, Expression.OrElse);
    }

    private static Expression<T> Compose<T>(
        this Expression<T> left,
        Expression<T> right,
        Func<Expression, Expression, Expression> merge)
    {
        var map = left.Parameters
            .Select((expr, index) => new { Expression = expr, Parameter = right.Parameters[index] })
            .ToDictionary(p => p.Parameter, p => p.Expression);

        var rightBody = ParameterRebinder.ReplaceParameters(map, right.Body);

        return Expression.Lambda<T>(merge(left.Body, rightBody), left.Parameters);
    }
}