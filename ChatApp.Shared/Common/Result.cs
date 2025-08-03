using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Result
{
    public class Result<T>
    {
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = true;
        public T? Data { get; set; }

        public Result()
        {
        }

        public Result(string message, bool isSuccess, T? data)
        {
            Message = message;
            IsSuccess = isSuccess;
            Data = data;
        }

        public static Result<T> Success(string message, T? data = default)
        {
            return new Result<T>(message, true, data);
        }

        public static Result<T> Failure(string message, T? data = default)
        {
            return new Result<T>(message, false, data);
        }
    }
}
