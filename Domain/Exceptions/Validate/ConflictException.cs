namespace ChatApp.Domain.Exceptions.Validate
{
    public class ConflictException: AppException
    {
        public override int StatusCode => 409; // Conflict
        public override string ErrorCode => "conflict_error";
        
        public ConflictException() : base(409, "The request could not be completed due to a conflict with the current state of the resource.")
        {
        }
        public ConflictException(string message) : base(409, message)
        {
        }
    }
}
