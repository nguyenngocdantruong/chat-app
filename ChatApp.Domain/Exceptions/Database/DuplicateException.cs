using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Database
{
    public class DuplicateException : AppException
    {
        public DuplicateException(string message) : base(message)
        {
        }

        public override int StatusCode => 409; // Conflict
        public override string ErrorCode => "DuplicateError";

        public DuplicateException(string message, Dictionary<string, string> errors) : base(message)
        {
            Errors = errors;
        }
    }
}
