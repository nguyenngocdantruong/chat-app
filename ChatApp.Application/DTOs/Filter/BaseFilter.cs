using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Filter
{
    public abstract class BaseFilter
    {
        public string? Keyword { get; set; }
        private int _page = 1;
        private int _pageSize = 10;
        public int Page { get => _page; set => _page = Math.Clamp(value, 1, int.MaxValue); }
        public int PageSize { get => _pageSize; set => _pageSize = Math.Clamp(value, 1, 20); }
    }
}
