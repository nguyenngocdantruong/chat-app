using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;

namespace ChatApp.Application.Interfaces.ExternalService
{
    public interface IFileService
    {
        Task<AttachmentResponseDto?> UploadFileAsync(AttachmentRequestDto requestDto);
        Task<AttachmentResponseDto?> GetFileMetadataAsync(AttachmentRequestDto requestDto);
        Stream? DownloadFileAsync(AttachmentRequestDto requestDto);
    }
}
