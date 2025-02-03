using Microsoft.EntityFrameworkCore.Storage;

namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncRepository
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
