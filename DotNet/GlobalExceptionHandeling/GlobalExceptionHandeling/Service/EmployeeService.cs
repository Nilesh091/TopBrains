using System;
using GlobalExceptionHandeling.Exceptions;
using GlobalExceptionHandeling.Models;

namespace GlobalExceptionHandeling.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new List<Employee>();

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await Task.FromResult(_employees);
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                throw new EmployeeNotFoundException(id);
            }
            return await Task.FromResult(employee);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _employees.Add(employee);
            return await Task.FromResult(employee);
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existingEmployee == null)
            {
                throw new EmployeeNotFoundException(employee.Id);
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Position = employee.Position;
            existingEmployee.Salary = employee.Salary;
            return await Task.FromResult(existingEmployee);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                throw new EmployeeNotFoundException(id);
            }

            _employees.Remove(employee);
            await Task.CompletedTask;
        }
    }
}
