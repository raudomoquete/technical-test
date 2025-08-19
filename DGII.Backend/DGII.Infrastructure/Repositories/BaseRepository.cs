using DGII.Application.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace DGII.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }

    public void ChangeEntityState(TEntity entity, EntityState state)
    {
        _dbContext.Entry(entity).State = state;
    }

    public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
    {
        if (predicate == null)
        {
            return _dbSet.Count();
        }
        else
        {
            return _dbSet.Count(predicate);
        }
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        if (predicate == null)
        {
            return await _dbSet.CountAsync();
        }
        else
        {
            return await _dbSet.CountAsync(predicate);
        }
    }

    public virtual long LongCount(Expression<Func<TEntity, bool>> predicate = null)
    {
        if (predicate == null)
        {
            return _dbSet.LongCount();
        }
        else
        {
            return _dbSet.LongCount(predicate);
        }
    }

    public virtual async Task<long> LongCountAsync(
        Expression<Func<TEntity, bool>> predicate = null
    )
    {
        if (predicate == null)
        {
            return await _dbSet.LongCountAsync();
        }
        else
        {
            return await _dbSet.LongCountAsync(predicate);
        }
    }

    public bool Exists(Expression<Func<TEntity, bool>> selector = null)
    {
        if (selector == null)
        {
            return _dbSet.Any();
        }
        else
        {
            return _dbSet.Any(selector);
        }
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> selector = null)
    {
        if (selector == null)
        {
            return await _dbSet.AnyAsync();
        }
        else
        {
            return await _dbSet.AnyAsync(selector);
        }
    }

    public virtual TEntity Find(params object[] keyValues)
    {
        return _dbSet.Find(keyValues: keyValues);
    }

    public virtual ValueTask<TEntity> FindAsync(params object[] keyValues)
    {
        return _dbSet.FindAsync(keyValues);
    }

    public virtual ValueTask<TEntity> FindAsync(
        object[] keyValues,
        CancellationToken cancellationToken
    )
    {
        return _dbSet.FindAsync(keyValues, cancellationToken);
    }

    public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
    {
        return _dbSet.FromSqlRaw(sql, parameters);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public IQueryable<TEntity> GetAll(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query);
        }
        else
        {
            return query;
        }
    }

    public IQueryable<TResult> GetAll<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query).Select(selector);
        }
        else
        {
            return query.Select(selector);
        }
    }

    public async Task<IList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public async Task<IList<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return await orderBy(query).Select(selector).ToListAsync();
        }
        else
        {
            return await query.Select(selector).ToListAsync();
        }
    }

    public virtual TEntity GetFirstOrDefault(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query).FirstOrDefault();
        }
        else
        {
            return query.FirstOrDefault();
        }
    }

    public TResult GetFirstOrDefault<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query).Select(selector).FirstOrDefault();
        }
        else
        {
            return query.Select(selector).FirstOrDefault();
        }
    }

    public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return await orderBy(query).Select(selector).FirstOrDefaultAsync();
        }
        else
        {
            return await query.Select(selector).FirstOrDefaultAsync();
        }
    }

    public virtual async Task<TEntity> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return await orderBy(query).FirstOrDefaultAsync();
        }
        else
        {
            return await query.FirstOrDefaultAsync();
        }
    }

    public IPagedList<TEntity> GetPagedList(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query).ToPagedList(pageIndex, pageSize);
        }
        else
        {
            return query.ToPagedList(pageIndex, pageSize);
        }
    }

    public IPagedList<TResult> GetPagedList<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        bool ignoreQueryFilters = false
    )
        where TResult : class
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query).Select(selector).ToPagedList(pageIndex, pageSize);
        }
        else
        {
            return query.Select(selector).ToPagedList(pageIndex, pageSize);
        }
    }

    public Task<IPagedList<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default,
        bool ignoreQueryFilters = false
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }
        else
        {
            return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }
    }

    public virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        int pageIndex = 0,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default,
        bool ignoreQueryFilters = false
    )
        where TResult : class
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return orderBy(query)
                .Select(selector)
                .ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }
        else
        {
            return query
                .Select(selector)
                .ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
        }
    }

    public virtual TEntity Insert(TEntity entity)
    {
        return _dbSet.Add(entity).Entity;
    }

    public virtual void Insert(params TEntity[] entities)
    {
        _dbSet.AddRangeAsync(entities);
    }

    public void Insert(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRangeAsync(entities);
    }

    public virtual ValueTask<EntityEntry<TEntity>> InsertAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        return _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual Task InsertAsync(params TEntity[] entities)
    {
        return _dbSet.AddRangeAsync(entities);
    }

    public virtual Task InsertAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        return _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Update(params TEntity[] entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Delete(object id)
    {
        // using a stub entity to mark for deletion
        var typeInfo = typeof(TEntity).GetTypeInfo();
        var key = _dbContext
            .Model
            .FindEntityType(typeInfo)
            .FindPrimaryKey()
            .Properties
            .FirstOrDefault();
        var property = typeInfo.GetProperty(key?.Name);
        if (property != null)
        {
            var entity = Activator.CreateInstance<TEntity>();
            property.SetValue(entity, id);
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }
        else
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }
    }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void Delete(params TEntity[] entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual void Delete(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}
