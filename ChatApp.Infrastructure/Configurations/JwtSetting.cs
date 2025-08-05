using ChatApp.Shared.Configurations;

namespace ChatApp.Infrastructure.Configurations
{
    public class JwtSetting: ITokenSetting
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60; 
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}
