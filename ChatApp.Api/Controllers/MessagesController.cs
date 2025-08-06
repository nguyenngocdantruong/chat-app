using ChatApp.Api.Response;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status404NotFound)]
    public class MessagesController(IMessageService messageService) : ControllerBase
    {
        [HttpGet("{conversationId}")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<MessageResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid conversationId)
        {
            var result = await messageService.GetMessagesByConversationIdAsync(conversationId);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            Result<MessageResponseDto> result = await messageService.GetMessageByIdAsync(id);
            if(result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
    }
}
