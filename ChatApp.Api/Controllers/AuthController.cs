using ChatApp.Api.Response;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Exceptions.Validate;
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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto? loginRequestDto)
        {
            if (loginRequestDto == null)
            {
                return ResponseJson.BadRequest();
            }
            // Login without 2FA code
            if (string.IsNullOrEmpty(loginRequestDto.Code) && string.IsNullOrEmpty(loginRequestDto.TransactionId))
            {
                var data = await _authService.LoginFirstStep(loginRequestDto);
                if (data.IsStepSuccess)
                {
                    return ResponseJson.MultiStatus(data, "Please input 2FA OTP", true);
                }
                else
                {
                    return ResponseJson.Unauthorized(null, "Invalid username or password", false);
                }
            }
            // Login with 2FA code
            else
            {
                var data = await _authService.LoginWith2FaAsync(loginRequestDto);
                if (data.IsStepSuccess)
                {
                    return ResponseJson.Ok(data, "Login successfully", true);
                }
                else
                {
                    return ResponseJson.Unauthorized(data, "Error while login with 2FA code", false);
                }
            }
        }
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ResponseDto<LoginResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDto? registerRequestDto, IFormFile? avatarFile)
        {
            if (registerRequestDto == null)
            {
                return ResponseJson.BadRequest();
            }
            AttachmentRequestDto? attachmentRequestDto = null;
            if (avatarFile != null && avatarFile.Length > 0)
            {
                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    await avatarFile.CopyToAsync(memoryStream);
                    bytes = memoryStream.ToArray();
                }
                attachmentRequestDto = new AttachmentRequestDto
                {
                    FileName = avatarFile.FileName,
                    ContentType = avatarFile.ContentType,
                    FileBytes = bytes
                };
            }
            var result = await _authService.RegisterAsync(registerRequestDto, attachmentRequestDto);
            if (result != null)
            {
                return ResponseJson.Created(result, "Registered successfully", true);
            }
            return ResponseJson.BadRequest(null, "Registration failed. Please check your input data.");
        }
    }
}
