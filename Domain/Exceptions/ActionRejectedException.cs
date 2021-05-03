using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ActionRejectedException : Exception
    {
        public ActionRejectedException(string? message) : base(message)
        {
        }

        public ActionRejectedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
