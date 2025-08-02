using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Request
{
    public class UpdateProfileRequestDto
    {
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? NewPassword { get; set; } = null!;

        public AttachmentRequestDto? AvatarFile { get; set; }

        [StringLength(100)]
        public string? DisplayName { get; set; }

        public bool? IsSearchable { get; set; } = true;
    }
}
