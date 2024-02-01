namespace Fusion.Infrastructure.Database.Abstractions;

public interface IRepository<in TKey, TEntity>
{
    Task<TEntity?> GetById(TKey id);
    Task Create(TEntity entity);
    Task Update(TKey id, TEntity entity);
    Task Delete(TKey id);
}