using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Filter
{
    public class FriendFilter: BaseFilter
    {
        public FriendStatus? Status { get; set; }
    }
}
