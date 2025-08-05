namespace ChatApp.Application.DTOs.Request
{
    public class FcmTokenRequestDto: BaseRequestDto
    {
        public string ? FcmToken { get; set; } // Firebase Cloud Messaging token
        public string ? DeviceType { get; set; }
        public bool? IsRevoked { get; set; } = false; // Indicates if the token is revoked
    }
}