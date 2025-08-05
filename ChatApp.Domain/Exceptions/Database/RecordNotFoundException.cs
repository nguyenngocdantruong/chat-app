using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Exceptions.Database
{
    public class RecordNotFoundException(string message) : AppException(message)
    {
        public RecordNotFoundException() : this("The requested resource was not found.")
        {
        }

        public override int StatusCode { get; } = 404;
        public override string ErrorCode { get; } = "NotFoundException";
    }
}
