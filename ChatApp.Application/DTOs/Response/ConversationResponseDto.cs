using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public partial class ConversationResponseDto : BaseResponseDto
    {
        public bool? IsGroup { get; set; }

        public string? Name { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Theme { get; set; }

        public string? Emoji { get; set; }

        public Guid? CreatedBy { get; set; }

        public ConversationSettingResponseDto? ConversationSettingForCurrentUser { get; set; }
    }

}