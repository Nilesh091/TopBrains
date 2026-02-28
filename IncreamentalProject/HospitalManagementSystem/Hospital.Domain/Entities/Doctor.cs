using System;

namespace Hospital.Core.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public int ConsultationFee { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}
