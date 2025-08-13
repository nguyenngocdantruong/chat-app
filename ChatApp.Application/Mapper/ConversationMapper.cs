using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Mapper
{
    public class ConversationMapper: IConversationMapper
    {
        public ConversationResponseDto MapToResponseDto(Conversation entity)
        {
            return new ConversationResponseDto()
            {
                AvatarUrl = entity.AvatarUrl,
            };
        }

        public ConversationListItemResponseDto MapToListItemResponse(Conversation conversation)
        {
            throw new NotImplementedException();
        }

        public Conversation MapToEntity(ConversationCreateRequestDto createRequestDto)
        {
            throw new NotImplementedException();
        }

        public Conversation MapToEntity(ConversationUpdateRequestDto updateRequestDto)
        {
            throw new NotImplementedException();
        }

        public ConversationMemberResponseDto MapToMemberResponse(ConversationMember conversationMember)
        {
            throw new NotImplementedException();
        }

        public ConversationSettingResponseDto MapToSettingResponse(ConversationSetting conversationSetting)
        {
            throw new NotImplementedException();
        }
    }
}
