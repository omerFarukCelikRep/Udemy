using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Udemy.Common.Models.Entities.Base;

namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncUpdateableRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    Task ExecuteUpdateAsync(Expression<Func<TEntity, bool>>? predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default);
    ValueTask<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}
