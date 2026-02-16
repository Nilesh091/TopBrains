using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class InvalidGPAException : Exception
    {
        public  InvalidGPAException (string message) : base (message) { }
    }
}
