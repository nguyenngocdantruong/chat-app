using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Services
{
    public class MessageService(IUnitOfWork uow, IMessageRepository repository, IMessageMapper mapper) : GenericService<Message, MessageResponseDto>(uow, repository, mapper), IMessageService
    {
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
