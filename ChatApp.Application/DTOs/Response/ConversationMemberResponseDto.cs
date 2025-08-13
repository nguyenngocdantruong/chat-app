using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Response
{
    public class ConversationMemberResponseDto : BaseResponseDto
    {
        public Guid? ConversationId { get; set; }

        public Guid? UserId { get; set; }

        public UserRole? Role { get; set; }
    }
}
