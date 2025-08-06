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
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status404NotFound)]
    public class FriendsController(IFriendService friendService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<IEnumerable<UserResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] FriendFilter filter)
        {
            PagedResult<FriendResponseDto> result = await friendService.Filter(filter);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] FriendUpdateRequestDto friendUpdateRequestDto)
        {
            var result = await friendService.UpdateFriendAsync(friendUpdateRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
    }
}
