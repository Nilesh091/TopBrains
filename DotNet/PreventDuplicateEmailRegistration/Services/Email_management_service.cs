using System;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PreventDuplicateEmailRegistration.DTOs;
using PreventDuplicateEmailRegistration.Models;
using PreventDuplicateEmailRegistration.Models.Context;

namespace PreventDuplicateEmailRegistration.Services
{
    public class Email_management_service
    {
        private readonly Context context = new Context();

        public void AddEmployee(EmployeeDTO emp)
        {
            if (context.Employees.Any(e => e.Email == emp.Email))
            {
                Console.Write("Email Already exist.");
                return;
            }
            var empl = new Employee
            {
                Email = emp.Email,
                Name = emp.Name

            };
            context.Employees.Add(empl);
            context.SaveChanges();
            Console.WriteLine("Employee added successfully.");
        }
    }
}
