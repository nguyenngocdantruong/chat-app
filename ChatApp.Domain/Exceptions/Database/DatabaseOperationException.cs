namespace ChatApp.Domain.Exceptions.Database;

public class DatabaseOperationException(string message): AppException(message)
{
    public DatabaseOperationException() : this("An error occurred while performing a database operation.")
    {
    }
    public override int StatusCode { get; } = 500;
    public override string ErrorCode { get; } = "DatabaseOperationError";
}