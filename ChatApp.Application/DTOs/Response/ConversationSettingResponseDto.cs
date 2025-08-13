using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public partial class ConversationSettingResponseDto : BaseResponseDto
    {
        public Guid? UserId { get; set; }

        public Guid? ConversationId { get; set; }

        public bool? MuteNotification { get; set; }

        public bool? Pinned { get; set; }
    }
}
