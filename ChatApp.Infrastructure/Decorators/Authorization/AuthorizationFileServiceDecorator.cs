using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Application.Interfaces.ExternalService;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Decorators.Authorization
{
    public class AuthorizationFileServiceDecorator(IFileService decoratedService, IAuthService authService, ICurrentUserService currentUserService, IAuthorizationHandler<Attachment> authorizationHandler) : AuthorizationDecoratorBaseWithoutCrud<Attachment>(authService, currentUserService, authorizationHandler), IFileService
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
