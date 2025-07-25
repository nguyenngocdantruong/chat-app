namespace ChatApp.Domain.Exceptions.Server
{
    public class ServiceUnavailableException: AppException
    {
        public override int StatusCode => 503; // Service Unavailable
        public override string ErrorCode => "service_unavailable";
        public ServiceUnavailableException() : base(503, "The service is currently unavailable. Please try again later.")
        {
        }
        public ServiceUnavailableException(string message) : base(503, message)
        {
        }
    }
}
