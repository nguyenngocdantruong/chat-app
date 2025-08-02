using Azure;
using ChatApp.Api.Response;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("me")]
        [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<object>),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDto<object>),StatusCodes.Status401Unauthorized)]
        public async Task<JsonResult> GetCurrentUser()
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value ?? throw new InvalidOperationException("UserId claim not found."));
            var user = await userService.GetCurrentUser(userId);
            if (user == null)
            {
                return new JsonResult(new { message = "User not found" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            return new JsonResult(user) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
