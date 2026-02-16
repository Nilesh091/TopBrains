using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class MedicineNotFoundException : Exception
    {
        public MedicineNotFoundException(string message) : base(message) { }
    }
}
