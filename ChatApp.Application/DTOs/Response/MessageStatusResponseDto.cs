using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Response
{
    public class MessageStatusResponseDto 
    {

        public Guid? MessageId { get; set; }
        public Guid? UserId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        public Reaction Reaction { get; set; }
    }
}
