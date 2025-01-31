using System.Linq.Expressions;
using Udemy.Common.Models.Entities.Base;

namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncDeleteableRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
}
