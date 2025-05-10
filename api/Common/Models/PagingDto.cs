using System;

namespace api.Common.Models;

public record PagingDto<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

}
