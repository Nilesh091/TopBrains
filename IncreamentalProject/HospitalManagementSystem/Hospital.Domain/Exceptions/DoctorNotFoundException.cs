using System;

namespace Hospital.Core.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(string message) : base(message)
        {

        }
    }
}
