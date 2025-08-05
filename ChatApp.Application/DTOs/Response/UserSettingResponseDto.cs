using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public partial class UserSettingResponseDto
    {
        public Guid? UserId { get; set; }

        public bool? MuteAllNotifications { get; set; }
        public bool? EnableE2e { get; set; }
        public string? ChatTheme { get; set; }
    }
}


