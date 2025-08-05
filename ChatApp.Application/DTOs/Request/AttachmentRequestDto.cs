using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Application.DTOs.Request
{
    public class AttachmentRequestDto
    {
        public string FileName { get; set; } = null!; // Name of the file to be uploaded
        public string ContentType { get; set; } = null!; // MIME type of the file
        public byte[] FileBytes { get; set; } = null!; // Byte array of the file content
    }
}