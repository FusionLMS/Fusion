using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Fusion.Infrastructure.Database.Specifications;

[ExcludeFromCodeCoverage]
public sealed class ParameterRebinder : ExpressionVisitor
{
    private readonly IDictionary<ParameterExpression, ParameterExpression> _map;

    private ParameterRebinder(IDictionary<ParameterExpression, ParameterExpression>? map)
    {
        _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    public static Expression ReplaceParameters(
        IDictionary<ParameterExpression, ParameterExpression> map,
        Expression expression)
    {
        return new ParameterRebinder(map).Visit(expression);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_map.TryGetValue(node, out var replacement)) node = replacement;

        return base.VisitParameter(node);
    }
}