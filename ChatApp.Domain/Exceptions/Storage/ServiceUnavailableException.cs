using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Storage
{
    public class ServiceUnavailableException(string message) : AppException(message)
    {
        public ServiceUnavailableException() : this(
            "The storage service is currently unavailable. Please try again later.")
        {
        }

        public override int StatusCode { get; } = 503; // HTTP status code for Service Unavailable
        public override string ErrorCode { get; } = "ServiceUnavailable";
    }
}
