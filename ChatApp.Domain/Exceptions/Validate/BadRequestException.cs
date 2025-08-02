using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Validate
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message)
        {
        }
        
        public BadRequestException(string message, params object[] args) : base(string.Format(message, args))
        {
        }

        public override int StatusCode { get; } = 400;
        public override string ErrorCode { get; } = "BadRequestException";
    }
}
