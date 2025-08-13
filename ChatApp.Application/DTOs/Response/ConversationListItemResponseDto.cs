using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Response
{
    public class ConversationListItemResponseDto : BaseResponseDto
    {
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public ConversationSettingResponseDto ConversationSettingForCurrentUser { get; set; } = new ConversationSettingResponseDto();
        public ConversationEventResponseDto? LastEventResponseDto { get; set; }
    }
}
