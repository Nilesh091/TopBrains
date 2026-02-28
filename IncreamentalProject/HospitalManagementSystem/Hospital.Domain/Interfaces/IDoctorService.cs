using System;
using Hospital.Core.Entities;
namespace Hospital.Core.Interfaces
{
    public interface IDoctorService
    {
        void AddDoctor(Doctor doctor);

        IEnumerable<Doctor> GetAllDoctors();

        Doctor GetDoctorById(int id);

        void UpdateDoctor(Doctor doctor);

        void DeleteDoctor(int id);
    }
}
