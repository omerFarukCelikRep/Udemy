using Udemy.Common.Models.Entities.Base;

namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncInsertableRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}
