using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingMessageServiceDecorator : LoggingBase<Friend, FriendResponseDto>, IMessageService
    {
        public Task<MessageResponseDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MessageResponseDto> CreateAsync(Message entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, Message entity)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MessageResponseDto>> GetMessagesByConversationIdAsync(Guid conversationId, int page = 1, int pageSize = 15)
        {
            throw new NotImplementedException();
        }

        public Task<Result<MessageResponseDto>> CreateMessageAsync(MessageRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<MessageResponseDto>> UpdateMessageAsync(MessageRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<object>> DeleteMessageAsync(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<object>> ReadMessageAsync(Guid userId, Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<MessageResponseDto>> GetMessageByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
