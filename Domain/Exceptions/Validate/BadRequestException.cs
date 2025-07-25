namespace ChatApp.Domain.Exceptions.Validate
{
    public class BadRequestException: AppException
    {
        public override int StatusCode => 400; // Bad Request
        public override string ErrorCode => "bad_request";
        public BadRequestException() : base(400, "The request was invalid or cannot be served.")
        {
        }
        public BadRequestException(string message) : base(400, message)
        {
        }
    }
}
