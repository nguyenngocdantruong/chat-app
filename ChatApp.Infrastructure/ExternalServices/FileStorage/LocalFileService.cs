using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.ExternalService;
using ChatApp.Domain.Exceptions.Storage;
using ChatApp.Domain.Exceptions.Validate;

namespace ChatApp.Infrastructure.ExternalServices.FileStorage
{
    public class LocalFileService: IFileService
    {
        private readonly string FolderPath = "D:/File";
        public async Task<AttachmentResponseDto?> UploadFileAsync(AttachmentRequestDto requestDto)
        {
            try
            {
                AttachmentResponseDto attachmentResponseDto = new AttachmentResponseDto
                {
                    FileType = requestDto.ContentType
                };
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                string fullPath = Path.Combine(FolderPath, requestDto.FileName);
                FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                await stream.WriteAsync(requestDto.FileBytes, 0, requestDto.FileBytes.Length);
                stream.Close();
                attachmentResponseDto.FileUrl = requestDto.FileName;
                attachmentResponseDto.FileSize = requestDto.FileBytes.LongLength;
                return attachmentResponseDto;
            }
            catch (Exception e)
            {
                throw new ServiceUnavailableException("Cannot upload file now");
            }
        }

        public async Task<AttachmentResponseDto?> GetFileMetadataAsync(AttachmentRequestDto requestDto)
        {
            AttachmentResponseDto response = new AttachmentResponseDto();
            string fullPath = Path.Combine(FolderPath, requestDto.FileName);
            if (!File.Exists(fullPath))
            {
                throw new ResourceNotFoundException("Cannot find the resource requested.");
            }

            FileInfo fileInfo = new FileInfo(fullPath);
            response.FileUrl = fileInfo.Name;
            response.FileSize = fileInfo.Length;
            response.FileType = fileInfo.Extension;
            return await Task.FromResult(response);
        }

        public Stream? DownloadFileAsync(AttachmentRequestDto requestDto)
        {
            string fullPath = Path.Combine(FolderPath, requestDto.FileName);
            if (!File.Exists(fullPath))
            {
                throw new ResourceNotFoundException("Cannot find the resource requested.");
            }

            FileStream stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return stream;
        }
    }
}
