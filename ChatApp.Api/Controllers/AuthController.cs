using ChatApp.Api.Response;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

namespace ChatApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseDto<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            // Login without 2FA code
            if (string.IsNullOrEmpty(loginRequestDto.Code) && string.IsNullOrEmpty(loginRequestDto.TransactionId))
            {
                var data = await _authService.LoginFirstStep(loginRequestDto);
                if (data.IsSuccess)
                {
                    return ResponseJson.MultiStatus(data.Data, data.Message, true);
                }
                else
                {
                    return ResponseJson.Unauthorized(null, data.Message, false);
                }
            }
            // Login with 2FA code
            else
            {
                var data = await _authService.LoginWith2FaAsync(loginRequestDto);
                if (data.IsSuccess )
                {
                    return ResponseJson.Ok(data.Data, "Login successfully", true);
                }
                else
                {
                    return ResponseJson.Unauthorized(data.Data, data.Message, false);
                }
            }
        }
        [HttpPost("pre-register")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PreRegisterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PreRegister([FromBody] PreRegisterRequestDto preRegisterRequestDto)
        {
            var result = await _authService.PreRegisterAsync(preRegisterRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ResponseDto<LoginResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDto registerRequestDto, IFormFile? avatarFile)
        {
            AttachmentRequestDto? attachmentRequestDto = null;
            if (avatarFile != null && avatarFile.Length > 0)
            {
                byte[] bytes = await FileConvertor.GetBytes(avatarFile);
                attachmentRequestDto = new AttachmentRequestDto
                {
                    FileName = avatarFile.FileName,
                    ContentType = avatarFile.ContentType,
                    FileBytes = bytes
                };
            }
            var result = await _authService.RegisterAsync(registerRequestDto, attachmentRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Created(result.Data, result.Message, true);
            }
            return ResponseJson.BadRequest(null, result.Message);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ResponseDto<TokenResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenRequestDto refreshAccessTokenRequestDto)
        {
            var result = await _authService.RefreshAccessTokenAsync(refreshAccessTokenRequestDto);
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }
            return ResponseJson.Unauthorized(null, result.Message, false);
        }
    }
}
