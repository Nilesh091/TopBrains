using System;

namespace Hospital.Core.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Condition { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }
    }
}
