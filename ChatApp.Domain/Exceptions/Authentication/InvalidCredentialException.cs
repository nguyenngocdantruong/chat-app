using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Authentication
{
    public class InvalidCredentialException(string message) : AppException(message)
    {
        public InvalidCredentialException() : this("Invalid credentials provided.")
        {
        }

        public override int StatusCode { get; } = 401; // Unauthorized
        public override string ErrorCode { get; } = "InvalidCredentialException";
    }
}
