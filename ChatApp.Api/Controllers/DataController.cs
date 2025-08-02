using ChatApp.Api.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.ExternalService;
using ChatApp.Infrastructure.ExternalServices.MailService;
using ChatApp.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController(ICacheService<string> cacheService, IMailService mailService)
    : ControllerBase
{
    [Authorize]
    [HttpGet("otp")]
    [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<>), StatusCodes.Status404NotFound)]
    public IActionResult GetOtp(string transactionId)
    {
        return ResponseJson.Ok(cacheService.Get(transactionId));
    }

    [HttpGet("email")]
    [ProducesResponseType(typeof(ResponseDto<string[]>), StatusCodes.Status200OK)]
    public IActionResult GetEmail([EmailAddress] string email)
    {
        if (mailService is MailConsoleService mailConsoleService)
        {
            return ResponseJson.Ok(mailConsoleService.GetInbox(email));
        }
        return ResponseJson.InternalServerError();
    }
}