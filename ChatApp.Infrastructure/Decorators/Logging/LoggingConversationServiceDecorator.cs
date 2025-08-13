using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingConversationServiceDecorator : LoggingBase<Conversation, ConversationResponseDto>, IConversationService
    {
        public Task<PagedResult<ConversationListItemResponseDto>> GetConversationsByUserIdAsync(ConversationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ConversationResponseDto>> CreateConversationAsync(ConversationCreateRequestDto createRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ConversationResponseDto>> UpdateConversationAsync(ConversationUpdateRequestDto updateRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Result<ConversationSettingResponseDto>>> GetConversationSettingByUser(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<object>> IsMemberInConversation(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ConversationMemberResponseDto>> GetMembersByConversationIdAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ConversationMemberResponseDto>> UpdateMemberConversationAsync(ConversationEventRequestDto<ConversationMemberRequestDto> conversationEventRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ConversationResponseDto>> GetConversationById(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<object>> DeleteConversationForUserAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }
    }
}
