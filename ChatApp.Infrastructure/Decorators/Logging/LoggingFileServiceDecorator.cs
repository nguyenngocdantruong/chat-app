using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.ExternalService;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingFileServiceDecorator : IFileService
    {
        public Task<AttachmentResponseDto?> UploadFileAsync(AttachmentRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<AttachmentResponseDto?> GetFileMetadataAsync(AttachmentRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Stream? DownloadFileAsync(AttachmentRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
