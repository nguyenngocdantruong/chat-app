namespace ChatApp.Domain.Exceptions.Server
{
    public class DatabaseOperationException: AppException
    {
        public override int StatusCode => 500; // Internal Server Error
        public override string ErrorCode => "database_operation_error";
        public DatabaseOperationException() : base(500, "An error occurred while performing a database operation.")
        {
        }
        public DatabaseOperationException(string message) : base(500, message)
        {
        }
    }
}
