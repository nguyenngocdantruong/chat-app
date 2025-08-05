using ChatApp.Api.Response;
using ChatApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers
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
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await userService.GetCurrentUser();
            if (result.IsSuccess)
            {
                return ResponseJson.Ok(result.Data, result.Message, true);
            }

            return ResponseJson.BadRequest(null, result.Message);
        }
    }
}
