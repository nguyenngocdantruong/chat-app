using System.Security.Claims;

namespace ChatApp.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(Guid uid, string email);
        string GenerateRefreshToken();
        ClaimsPrincipal? Validate(string token);

    }
}
