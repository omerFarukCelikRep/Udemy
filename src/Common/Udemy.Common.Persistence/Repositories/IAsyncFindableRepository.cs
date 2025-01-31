using System.Linq.Expressions;
using Udemy.Common.Models.Entities.Base;

namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncFindableRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TId id, bool tracking = true, CancellationToken cancellationToken = default);
    Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, CancellationToken cancellationToken = default);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, CancellationToken cancellationToken = default);
}
