using ChatApp.Api.Response;
using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status404NotFound)]
    public class ConversationsController(IConversationService conversationService): ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ConversationListItemResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConversationsAsync([FromQuery] ConversationFilter filter)
        {
            PagedResult<ConversationListItemResponseDto> result =
                await conversationService.GetConversationsByUserIdAsync(filter);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }

            return ResponseJson.BadRequest(null, result.Message);
        }
        [HttpGet("{conversationId}")]
        [ProducesResponseType(typeof(ResponseDto<ConversationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConversationByIdAsync(Guid conversationId)
        {
            Result<ConversationResponseDto> result = await conversationService.GetConversationById(conversationId);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<ConversationResponseDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateConversationAsync([FromBody] ConversationCreateRequestDto createRequestDto)
        {
            Result<ConversationResponseDto> result = await conversationService.CreateConversationAsync(createRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Created(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<ConversationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateConversationAsync([FromBody] ConversationUpdateRequestDto updateRequestDto)
        {
            Result<ConversationResponseDto> result = await conversationService.UpdateConversationAsync(updateRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
        [HttpGet("{conversationId}/settings")]
        [ProducesResponseType(typeof(ResponseDto<ConversationSettingResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConversationSettingByUserAsync(Guid conversationId, Guid userId)
        {
            Result<Result<ConversationSettingResponseDto>> result = await conversationService.GetConversationSettingByUser(conversationId, userId);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteConversationAsync(Guid conversationId)
        {
            Result<object> result = await conversationService.DeleteConversationForUserAsync(conversationId);
            if (result.IsSuccess)
            {
                return ResponseJson.NoContent();
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpGet("{conversationId}/members")]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<ConversationMemberResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMembersByConversationIdAsync(Guid conversationId)
        {
            PagedResult<ConversationMemberResponseDto> result = await conversationService.GetMembersByConversationIdAsync(conversationId);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }


        [HttpPost("{conversationId}/members")]
        [ProducesResponseType(typeof(ResponseDto<ConversationMemberResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMember(Guid conversationId, [FromBody] ConversationEventRequestDto<ConversationMemberRequestDto> conversationEventRequestDto)
        {
            Result<ConversationMemberResponseDto> result = await conversationService.UpdateMemberConversationAsync(conversationEventRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
    }
}
