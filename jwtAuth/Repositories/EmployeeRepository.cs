using System;
using jwtAuth.Models;

namespace jwtAuth.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees = new();

        public IEnumerable<Employee> GetAllEmployees() => _employees;

        public Employee GetEmployeeById(int id) =>
            _employees.FirstOrDefault(e => e.Id == id);

        public void AddEmployee(Employee employee)
        {
            employee.Id = _employees.Count + 1;
            _employees.Add(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            var existing = GetEmployeeById(employee.Id);
            if (existing != null)
            {
                existing.Name = employee.Name;
                existing.Address = employee.Address;
                existing.Gender = employee.Gender;
                existing.Company = employee.Company;
                existing.Designation = employee.Designation;
            }
        }

        public void DeleteEmployee(int id)
        {
            _employees.RemoveAll(e => e.Id == id);
        }
    }
}
