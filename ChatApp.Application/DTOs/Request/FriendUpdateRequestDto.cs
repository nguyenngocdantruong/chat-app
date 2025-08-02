using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Request
{
    public class FriendUpdateRequestDto: BaseRequestDto
    {
        public Guid? RequesterId { get; set; }
        public Guid? AddresseeId { get; set; }
        public FriendStatus? Status { get; set; }
    }
}
