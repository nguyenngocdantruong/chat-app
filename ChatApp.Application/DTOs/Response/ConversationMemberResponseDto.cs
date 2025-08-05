using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Response
{
    public partial class ConversationMemberResponseDto
    {
        public Guid Id { get; set; }
        public Guid? ConversationId { get; set; }

        public Guid? UserId { get; set; }

        public UserRole? Role { get; set; }
    }
}
