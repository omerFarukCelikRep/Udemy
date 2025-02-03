using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Udemy.Common.Models.Entities.Base;
using Udemy.Common.Models.Models.Pagination;
using Udemy.Common.Persistence.Extensions;

namespace Udemy.Common.Persistence.Repositories.Base;
public abstract class EFBaseRepository<TEntity, TId>(DbContext context) :
    IAsyncInsertableRepository<TEntity, TId>,
    IAsyncUpdateableRepository<TEntity, TId>,
    IAsyncDeleteableRepository<TEntity, TId>,
    IAsyncQueryableRepository<TEntity, TId>,
    IAsyncOrderableRepository<TEntity, TId>,
    IAsyncFindableRepository<TEntity, TId>,
    IAsyncPaginateRepository<TEntity, TId>,
    IAsyncRepository
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    private readonly DbSet<TEntity> _table = context.Set<TEntity>();

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _table.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _table.AddRangeAsync(entities, cancellationToken);
    }

    public ValueTask<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        EntityEntry<TEntity> entry = _table.Update(entity);
        return new ValueTask<TEntity>(entry.Entity);
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _table.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public async Task ExecuteUpdateAsync(Expression<Func<TEntity, bool>>? predicate,
                                         Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
                                         CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<TEntity> queryable = GetAllAsQueryable();
        if (predicate is not null)
            queryable = queryable.Where(predicate);

        await queryable.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
    }

    public async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        TEntity entity = await _table.SingleAsync(e => e.Id.Equals(id), cancellationToken: cancellationToken);
        _table.Remove(entity);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _table.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _table.Where(expression).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return expression is null
            ? await _table.AnyAsync(cancellationToken)
            : await _table.AnyAsync(expression, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id,
                                             bool tracking = true,
                                             CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await FirstOrDefaultAsync(x => x.Id.Equals(id), tracking, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
                                                    bool tracking = true,
                                                    CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<TEntity> query = GetAllAsQueryable(tracking);

        return await query.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
                                                     bool tracking = true,
                                                     CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetAllAsQueryable(tracking).SingleOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
                                                   bool tracking = true,
                                                   CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetAllAsQueryable(tracking).LastOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetAllAsQueryable(tracking).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression,
                                                        bool tracking = true,
                                                        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetAllAsQueryable(tracking).Where(expression)
                                                .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy,
                                                              bool orderByDesc = false,
                                                              bool tracking = true,
                                                              CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<TEntity> query = GetAllAsQueryable(tracking);
        IOrderedQueryable<TEntity> orderedQuery = !orderByDesc
            ? query.OrderBy(orderBy)
            : query.OrderByDescending(orderBy);

        return await orderedQuery.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, bool>> expression,
                                                              Expression<Func<TEntity, TKey>> orderBy,
                                                              bool orderByDesc = false,
                                                              bool tracking = true,
                                                              CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<TEntity> query = GetAllAsQueryable(tracking).Where(expression);
        IOrderedQueryable<TEntity> orderedQuery = !orderByDesc
            ? query.OrderBy(orderBy)
            : query.OrderByDescending(orderBy);

        return await orderedQuery.ToListAsync(cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetAllAsPaginateAsync(int index = 1,
                                                               int size = 10,
                                                               bool tracking = true,
                                                               CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetAllAsQueryable(tracking).ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetAllAsPaginateAsync(Expression<Func<TEntity, bool>> expression,
                                                               int index = 1,
                                                               int size = 10,
                                                               bool tracking = true,
                                                               CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetAllAsQueryable(tracking).Where(expression)
                                                .ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetAllAsPaginateAsync<TKey>(Expression<Func<TEntity, bool>> expression,
                                                                     Expression<Func<TEntity, TKey>> orderBy,
                                                                     bool orderByDesc = false,
                                                                     int index = 1,
                                                                     int size = 10,
                                                                     bool tracking = true,
                                                                     CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IQueryable<TEntity> query = GetAllAsQueryable(tracking).Where(expression);
        IOrderedQueryable<TEntity> orderedQuery = !orderByDesc
            ? query.OrderBy(orderBy)
            : query.OrderByDescending(orderBy);

        return await orderedQuery.ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await context.SaveChangesAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => await context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default) => await context.Database.CommitTransactionAsync(cancellationToken);

    /// <summary>
    /// Get All Data as IQueryable
    /// </summary>
    /// <param name="tracking">Tracking Value</param>
    /// <returns><see cref="IQueryable{TEntity}"/></returns>
    protected IQueryable<TEntity> GetAllAsQueryable(bool tracking = true)
    {
        IQueryable<TEntity> values = _table.AsQueryable<TEntity>();

        return tracking
            ? values
            : values.AsNoTracking();
    }
}
