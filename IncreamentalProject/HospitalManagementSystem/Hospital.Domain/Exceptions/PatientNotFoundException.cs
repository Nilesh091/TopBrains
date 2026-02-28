using System;

namespace Hospital.Core.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message)
        {

        }
    }
}
