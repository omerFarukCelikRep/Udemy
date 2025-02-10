using Microsoft.EntityFrameworkCore;
using Udemy.Common.Models.Models.Pagination;

namespace Udemy.Common.Persistence.Extensions;
public static class IQueryablePaginateExtensions
{
    public static async Task<Paginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);

        EnsureInRange(index);
        EnsureInRange(size);

        int count = await source.CountAsync(cancellationToken)
                                .ConfigureAwait(false);

        IQueryable<T> items = source.Take((index * size)..size);

        return new Paginate<T>(items, index, size, count);
    }

    private static void EnsureInRange(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        ArgumentOutOfRangeException.ThrowIfLessThan(value, default);
    }
}
