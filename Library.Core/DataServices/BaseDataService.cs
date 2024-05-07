using System.Linq.Expressions;
using Library.Data;
using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.DataServices;

public abstract class BaseDataService<TEntity, TKey>(ApplicationDbContext applicationDbContext)
    where TEntity : BaseEntity<TKey>
{
    public abstract Task InitializeAsync();

    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await applicationDbContext.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        entity.RowCreatedUtc = DateTime.UtcNow;
        entity.RowModifiedUtc = DateTime.UtcNow;
        await applicationDbContext.Set<TEntity>().AddAsync(entity);
        await applicationDbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.RowModifiedUtc = DateTime.UtcNow;
        applicationDbContext.Set<TEntity>().Update(entity);
        await applicationDbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            applicationDbContext.Set<TEntity>().Remove(entity);
            await applicationDbContext.SaveChangesAsync();
        }
    }
    
    public virtual async Task SoftDeleteAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.RowDeletedUtc = DateTime.UtcNow;
            await UpdateAsync(entity);
        }
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await applicationDbContext.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return await applicationDbContext.Set<TEntity>().Select(selector).ToListAsync();
    }

    public virtual IQueryable<TEntity> GetAllAsQueryable()
    {
        return applicationDbContext.Set<TEntity>().AsNoTracking();
    }


    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await applicationDbContext.Set<TEntity>().Where(expression).ToListAsync();
    }

    public virtual async Task<List<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, TResult>> selector)
    {
        return await applicationDbContext.Set<TEntity>().Where(expression).Select(selector).ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, int skip, int take)
    {
        return await applicationDbContext.Set<TEntity>().Where(expression).Skip(skip).Take(take).ToListAsync();
    }


    public virtual async Task<TEntity> CreateOrUpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression)
    {
        if (entity == null)
        {
            throw new InvalidOperationException();
        }

        var existing = await applicationDbContext.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(expression);

        if (existing == null)
        {
            entity = await CreateAsync(entity);
        }
        else
        {
            entity.RowCreatedUtc = existing.RowCreatedUtc;
            entity.RowModifiedUtc = existing.RowModifiedUtc;
            entity.Id = existing.Id;
            entity = await UpdateAsync(entity);
        }

        return entity;
    }

    public virtual async Task BulkInsertAsync(IEnumerable<TEntity> entities, int batchSize = 100)
    {
        var utcNow = DateTime.UtcNow;
        
        var baseEntities = new List<TEntity>();
        
        foreach (var entity in entities)
        {
            entity.RowCreatedUtc = utcNow;
            entity.RowModifiedUtc = utcNow;
            
            baseEntities.Add(entity);
        }
        
        await applicationDbContext.BulkInsertAsync(
            baseEntities, 
            opt => opt.BatchSize = batchSize);
    }

    public virtual async Task<int> CountAsync()
    {
        return await applicationDbContext.Set<TEntity>().CountAsync();
    }
    
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await applicationDbContext.Set<TEntity>().CountAsync(expression);
    }
}