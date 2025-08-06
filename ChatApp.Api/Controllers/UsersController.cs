using System.ComponentModel.DataAnnotations;
using ChatApp.Api.Response;
using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Shared.Common;
using ChatApp.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto<PagedResult<UserResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchUser([FromQuery] UserFilter paginationRequestDto)
        {
            var result = await userService.Filter(paginationRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }
        [HttpGet("me")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await userService.GetCurrentUser();
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }

            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpPut("me")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateProfileRequestDto userUpdateRequestDto, IFormFile? fileImage)
        {
            if (fileImage != null)
            {
                byte[] bytes = await FileConvertor.GetBytes(fileImage);
                AttachmentRequestDto attachmentRequest = new AttachmentRequestDto()
                {
                    ContentType = fileImage.ContentType,
                    FileName = fileImage.FileName,
                    FileBytes = bytes
                };
                userUpdateRequestDto.AvatarFile = attachmentRequest;
            }
            var result = await userService.UpdateProfileAsync(userUpdateRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpPost("me/fcm-token")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterFcmToken([FromBody] FcmTokenRequestDto fcmTokenRequestDto)
        {
            var currentUser = await userService.GetCurrentUser();
            if (!currentUser.IsSuccess)
            {
                return ResponseJson.BadRequest(null, currentUser.Message);
            }

            var result = await userService.RegisterFcmTokenAsync(currentUser.Data!.Guid, fcmTokenRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpDelete("me/fcm-token")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnregisterFcmToken([FromBody] FcmTokenRequestDto fcmTokenRequestDto)
        {
            var currentUser = await userService.GetCurrentUser();
            if (!currentUser.IsSuccess)
            {
                return ResponseJson.BadRequest(null, currentUser.Message);
            }

            var result = await userService.UnregisterFcmTokenAsync(currentUser.Data!.Guid, fcmTokenRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpDelete("me")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAccount()
        {
            var result = await userService.DeleteAccountAsync();
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpGet("{uid}")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUid(Guid uid)
        {
            var result = await userService.GetByUid(uid, isFromAuthAction: false);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.NotFound(null, result.Message);
        }


        [HttpGet("username")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var result = await userService.GetByUsername(username, isFromAuthAction: false);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.NotFound(null, result.Message);
        }

        [HttpGet("email")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail([EmailAddress] string email)
        {
            var result = await userService.GetByEmailAsync(email, isFromAuthAction: false);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.NotFound(null, result.Message);
        }
    }
}
