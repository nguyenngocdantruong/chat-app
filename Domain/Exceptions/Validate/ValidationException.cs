namespace ChatApp.Domain.Exceptions.Validate
{
    public class ValidationException: AppException
    {
        public override int StatusCode => 422; // Unprocessable Entity
        public override string ErrorCode => "validation_error";
        public ValidationException() : base(422, "The request contains invalid data.")
        {
        }
        public ValidationException(string message) : base(422, message)
        {
        }
    }
}
