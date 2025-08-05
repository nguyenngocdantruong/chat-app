namespace ChatApp.Shared.Common
{
    public class Result<T>
    {
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = true;
        public T? Data { get; set; }

        public int Code { get; set; } = 200; // Default to 200 OK

        public Result()
        {
        }

        public Result(string message, bool isSuccess, T? data)
        {
            Message = message;
            IsSuccess = isSuccess;
            Data = data;
        }

        public static Result<T> Success(string message, T? data = default, int code = 200)
        {
            return new Result<T>(message, true, data);
        }

        public static Result<T> Failure(string message, T? data = default, int code = 400)
        {
            return new Result<T>(message, false, data);
        }
    }
}
