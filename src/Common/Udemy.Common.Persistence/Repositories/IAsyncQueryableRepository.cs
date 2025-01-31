using System.Linq.Expressions;
using Udemy.Common.Models.Entities.Base;

namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncQueryableRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, CancellationToken cancellationToken = default);
}
