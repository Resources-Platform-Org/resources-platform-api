using System;

namespace Api.Dto;
public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

    // Derived metadata
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    // Factory helper
    public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        return new PagedResult<T>
        {
            Items = (items as IReadOnlyList<T>) ?? items.ToList(),
            TotalCount = totalCount < 0 ? 0 : totalCount,
            Page = page < 1 ? 1 : page,
            PageSize = pageSize < 1 ? 1 : pageSize
        };
    }

    public static PagedResult<T> Empty(int page = 1, int pageSize = 20) =>
        new PagedResult<T> { Page = page, PageSize = pageSize };
}
