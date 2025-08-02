
namespace ChatApp.Application.DTOs.Response
{
    public partial class AttachmentResponseDto
    {
        public Guid? MessageId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileType { get; set; }
        public long? FileSize { get; set; }
    }
}