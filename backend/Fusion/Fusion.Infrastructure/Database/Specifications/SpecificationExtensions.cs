namespace Fusion.Infrastructure.Database.Specifications;

public static class SpecificationExtensions
{
    public static IEnumerable<Specification<TEntity>> Map<TEntity, TValue>(
        this TValue[] values,
        Func<TValue, Specification<TEntity>> selector,
        bool defaultValue = true)
    {
        return values.Length is 0
            ? Enumerable.Repeat(FromBool<TEntity>(defaultValue), 1)
            : values.Select(selector);
    }

    public static Specification<T> CombineAnd<T>(this IEnumerable<Specification<T>> specs)
    {
        return specs.Aggregate((seed, spec) => seed &= spec);
    }

    public static Specification<T> CombineOr<T>(this IEnumerable<Specification<T>> specs)
    {
        return specs.Aggregate((seed, spec) => seed |= spec);
    }

    private static Specification<T> FromBool<T>(bool defaultValue)
    {
        return defaultValue
            ? Specification<T>.True
            : Specification<T>.False;
    }
}