using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions
{
    public abstract class AppException(string message) : Exception(message)
    {
        public abstract int StatusCode { get; }
        public abstract string ErrorCode { get; }

        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
