using Fusion.Infrastructure.Database.Specifications;

namespace Fusion.Infrastructure.Database.Abstractions;

public interface IRepository<in TKey, TEntity>
{
    Task<TEntity?> GetById(TKey id);
    Task<List<TEntity>> GetBySpecification(Specification<TEntity> spec);
    Task<bool> ExistsBySpecification(Specification<TEntity> spec);
    Task<TEntity> Create(TEntity entity);
    Task Update(TKey id, TEntity entity);
    Task Delete(TKey id);
}