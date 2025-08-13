using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public class UserSettingResponseDto : BaseResponseDto
    {
        public Guid? UserId { get; set; }

        public bool? MuteAllNotifications { get; set; }
        public bool? EnableE2E { get; set; }
        public string? ChatTheme { get; set; }
    }
}


