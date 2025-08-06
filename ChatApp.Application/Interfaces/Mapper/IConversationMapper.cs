using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Mapper
{
    public interface IConversationMapper: IDtoMapper<Conversation, ConversationResponseDto>
    {
        ConversationListItemResponseDto MapToListItemResponse(Conversation conversation);
        Conversation MapToEntity(ConversationCreateRequestDto createRequestDto);
        Conversation MapToEntity(ConversationUpdateRequestDto updateRequestDto);
        ConversationMemberResponseDto MapToMemberResponse(ConversationMember conversationMember);
        ConversationSettingResponseDto MapToSettingResponse(ConversationSetting conversationSetting);
    }
}
