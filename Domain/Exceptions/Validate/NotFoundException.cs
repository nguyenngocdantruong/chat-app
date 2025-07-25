namespace ChatApp.Domain.Exceptions.Validate
{
    public class NotFoundException: AppException
    {
        public override int StatusCode => 404; // Not Found
        public override string ErrorCode => "not_found_error";
        public NotFoundException() : base(404, "The requested resource was not found.")
        {
        }
        public NotFoundException(string message) : base(404, message)
        {
        }
    }
}
