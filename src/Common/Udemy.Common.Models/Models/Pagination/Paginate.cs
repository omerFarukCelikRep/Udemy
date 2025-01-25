namespace Udemy.Common.Models.Models.Pagination;

public class Paginate<TModel>
{
    public Paginate(IEnumerable<TModel> items, int pageNumber, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(items);

        Index = pageNumber;
        Size = pageSize;

        if (items is IQueryable<TModel> queryable)
        {
            Count = queryable.Count();
            Items = queryable.Take((Index * Size)..Size).ToList().AsReadOnly();
        }
        else
        {
            TModel[] enumerable = items as TModel[] ?? [.. items];
            Count = enumerable.Length;
            Items = enumerable.Take((Index * Size)..Size).ToList().AsReadOnly();
        }

        Pages = (int)Math.Ceiling(Count / (double)pageSize);
    }

    public int Index { get; init; }
    public int Size { get; init; }
    public int Count { get; init; }
    public int Pages { get; init; }
    public IReadOnlyCollection<TModel> Items { get; init; }
    public bool HasPrevious => Index * Size > 0;
    public bool HasNext => Index < Pages;
}