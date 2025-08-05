using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Validate
{
    public class NullValueException(string message) : AppException(message)
    {
        public NullValueException() : this("The request data cannot be null") { }
        public override int StatusCode => 400; // Bad Request
        public override string ErrorCode => "NullValueException";
    }
}
