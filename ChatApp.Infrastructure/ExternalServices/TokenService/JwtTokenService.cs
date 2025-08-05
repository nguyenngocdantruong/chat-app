using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Shared.Configurations;

namespace ChatApp.Infrastructure.ExternalServices.TokenService
{
    public class JwtTokenService(ITokenSetting jwtSetting) : ITokenService
    {
        private readonly ITokenSetting _jwtSettings = jwtSetting ?? throw new ArgumentNullException(nameof(jwtSetting));

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string GenerateToken(Guid uid, string email)
        {
            // Validate JWT settings
            if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
            {
                throw new InvalidOperationException("JWT SecretKey is null or empty!");
            }
            
            if (string.IsNullOrEmpty(_jwtSettings.Issuer))
            {
                throw new InvalidOperationException("JWT Issuer is null or empty!");
            }
            
            if (string.IsNullOrEmpty(_jwtSettings.Audience))
            {
                throw new InvalidOperationException("JWT Audience is null or empty!");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, uid.ToString()),
                new Claim(ClaimTypes.Email, email),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? Validate(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null; 
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero 
                }, out SecurityToken validatedToken);

                // Kiểm tra token là JwtSecurityToken
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase)) // Kiểm tra nếu Token không dùng thuật toán HMAC SHA256
                {
                    return null; 
                }

                return principal;
            }
            catch (SecurityTokenExpiredException) // Token hết hạn
            {
                return null; 
            }
            catch (SecurityTokenInvalidSignatureException) // Chữ ký không hợp lệ
            {
                return null; 
            }
            catch (Exception) // Các lỗi khác (issuer, audience không khớp, v.v.)
            {
                return null; 
            }
        }
    }
}
