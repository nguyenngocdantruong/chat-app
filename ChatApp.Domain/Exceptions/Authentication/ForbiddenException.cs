using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Authentication
{
    public class ForbiddenException(string message) : AppException(message)
    {
        public override int StatusCode { get; } = 403;
        public override string ErrorCode { get; } = "ForbiddenException";

        public ForbiddenException() : this("You do not have permission to perform this action.")
        {
        }
    }
}
