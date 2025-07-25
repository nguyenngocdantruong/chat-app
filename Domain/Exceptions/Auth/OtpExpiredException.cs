namespace ChatApp.Domain.Exceptions.Auth
{
    public class OtpExpiredException: AppException
    {
        public override int StatusCode => 410; // Gone
        public override string ErrorCode => "otp_expired";
        
        public OtpExpiredException() : base(410, "The OTP has expired.")
        {
        }
        public OtpExpiredException(string message) : base(410, message)
        {
        }
    }
}
