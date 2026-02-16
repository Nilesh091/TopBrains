using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class InvalidExpiryYearException : Exception
    {
        public InvalidExpiryYearException(string message) : base(message) { }
    }
}
