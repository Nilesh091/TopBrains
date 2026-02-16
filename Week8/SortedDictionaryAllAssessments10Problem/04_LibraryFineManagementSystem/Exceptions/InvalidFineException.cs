using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class InvalidFineException : Exception
    {
        public InvalidFineException(string message) : base(message) { }
    }
}
