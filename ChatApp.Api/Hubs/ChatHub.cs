using ChatApp.Application.DTOs.Request;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs
{
    [Authorize]
    public class ChatHub
        (ICurrentUserService currentUserService,
            IConversationService conversationService,
            IMessageService messageService)
        : Hub
    {

        public async Task SendMessage(MessageRequestDto messageRequest)
        {
            Guid userId = currentUserService.UserId.Value;

        }
    }
}
