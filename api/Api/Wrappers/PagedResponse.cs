namespace Api.Wrappers;

public class PagedResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
        : base(data, null)
    {
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.TotalRecords = totalRecords;
        this.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
    }
}