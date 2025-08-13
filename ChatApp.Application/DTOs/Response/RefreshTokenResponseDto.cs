using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Response
{
    public class RefreshTokenResponseDto : BaseResponseDto
    {
        public Guid? UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}
