using Fusion.Infrastructure.Database.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Infrastructure.Database.Repositories;

public class RepositoryBase<TKey, TEntity, TContext>(TContext dbContext)
    : IRepository<TKey, TEntity>
    where TContext : DbContext
    where TEntity : BaseEntity<TKey>
{
    public async Task<TEntity?> GetById(TKey id)
    {
        return await dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }

    public async Task Create(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(TKey id, TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(TKey id)
    {
        var entity = await GetById(id);
        if (entity is null)
        {
            return;
        }

        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}