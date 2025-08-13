using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Response
{
    public abstract class BaseResponseDto
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
