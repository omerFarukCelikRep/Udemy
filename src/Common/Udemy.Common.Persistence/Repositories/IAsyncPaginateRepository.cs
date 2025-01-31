using System.Linq.Expressions;
using Udemy.Common.Models.Entities.Base;
using Udemy.Common.Models.Models.Pagination;

namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncPaginateRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{
    Task<Paginate<TEntity>> GetAllAsPaginateAsync(int index = 1, int size = 10, bool tracking = true, CancellationToken cancellationToken = default);
    Task<Paginate<TEntity>> GetAllAsPaginateAsync(Expression<Func<TEntity, bool>> expression, int index = 1, int size = 10, bool tracking = true, CancellationToken cancellationToken = default);
    Task<Paginate<TEntity>> GetAllAsPaginateAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, bool orderByDesc = false, int index = 1, int size = 10, bool tracking = true, CancellationToken cancellationToken = default);
}
