using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Validate
{
    public class ValidationException(string message) : AppException(message)
    {
        public ValidationException() : this("Validation error occurred.")
        {
        }

        public ValidationException(IEnumerable<string> errors) : this(string.Join(", ", errors))
        {
        }

        public override int StatusCode { get; } = 400; // Bad Request
        public override string ErrorCode { get; }  = "ValidationError";
    }
}
