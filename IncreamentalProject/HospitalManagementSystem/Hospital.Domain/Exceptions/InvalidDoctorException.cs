using System;

namespace Hospital.Core.Exceptions
{
    public class InvalidDoctorException : Exception
    {
        public InvalidDoctorException(string message) : base(message)
        {

        }
    }
}
