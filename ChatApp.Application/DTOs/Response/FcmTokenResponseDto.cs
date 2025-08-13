using ChatApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public class FcmTokenResponseDto : BaseResponseDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public bool IsRevoked { get; set; } = false;
    }
}
