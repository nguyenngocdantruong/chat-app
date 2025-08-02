using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Runtime
{
    public class ArgumentNullException: AppException
    {
        public ArgumentNullException(string message) : base(message)
        {
        }
        
        public ArgumentNullException(string paramName, string message) : base($"{message} (Parameter: {paramName})")
        {
        }
        public override int StatusCode { get; } = 500; // Internal Server Error
        public override string ErrorCode { get; } = "ArgumentNullException";
    }
}
