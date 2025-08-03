using ChatApp.Application.DTOs.Result;

namespace ChatApp.Shared.Common
{
    public class PagedResult<T> : Result<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; } = [];

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public PagedResult()
        {
        }
        public PagedResult(int page, int pageSize, int totalCount, IEnumerable<T> items, string message = "", bool isSuccess = true)
            : base(message, isSuccess, default)
        {
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }
    }
}
