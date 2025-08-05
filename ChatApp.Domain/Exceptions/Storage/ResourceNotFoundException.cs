using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Storage
{
    public class ResourceNotFoundException(string message) : AppException(message)
    {
        public ResourceNotFoundException() : this("The requested resource was not found.")
        {
        }

        public override int StatusCode { get; } = 404; // Not Found
        public override string ErrorCode { get; } = "ResourceNotFoundException";
    }
}
