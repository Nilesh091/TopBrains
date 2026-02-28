using System;
using Hospital.Core.Entities;
namespace Hospital.Core.Interfaces
{
    public interface IPatientService
    {
        void AddPatient(Patient patient);

        IEnumerable<Patient> GetAllPatients();

        Patient GetPatientById(int id);

        IEnumerable<Patient> FindPatientByName(string name);

        void UpdatePatient(Patient patient);

        void DeletePatient(int id);
    }
}
