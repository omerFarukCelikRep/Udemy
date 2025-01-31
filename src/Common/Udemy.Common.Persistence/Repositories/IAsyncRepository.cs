namespace Udemy.Common.Persistence.Repositories;
public interface IAsyncRepository
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
