namespace ChatApp.Domain.Exceptions.Server
{
    public class RateLimitExceededException: AppException
    {
        public override int StatusCode => 429; // Too Many Requests
        public override string ErrorCode => "rate_limit_exceeded";
        public RateLimitExceededException() : base(429, "Rate limit exceeded. Please try again later.")
        {
        }
        public RateLimitExceededException(string message) : base(429, message)
        {
        }
    }
}
