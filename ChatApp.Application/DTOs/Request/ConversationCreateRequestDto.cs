using ChatApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Request
{
    public class ConversationCreateRequestDto
    {
        [StringLength(100, ErrorMessage = "The room name is max 100 characters.")]
        public string? Name { get; set; }
        public bool? IsGroup => Members?.Count > 1;

        public AttachmentRequestDto? AvatarFile { get; set; }

        [StringLength(50)]
        public string? Theme { get; set; } = "light";

        [StringLength(50)]
        public string? Emoji { get; set; } = "default";

        public List<Guid> Members { get; set; } = new List<Guid>();
    }
}