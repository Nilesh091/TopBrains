// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using Microsoft.EntityFrameworkCore;
using PreventDuplicateEmailRegistration.DTOs;
using PreventDuplicateEmailRegistration.Models;
using PreventDuplicateEmailRegistration.Services;

class Program
{
  static void Main(string[] args)
  {
    Email_management_service service = new Email_management_service();

    Console.WriteLine("Enter the name.");
    string name = Console.ReadLine();
    Console.WriteLine("Enter the email.");
    string email = Console.ReadLine();
    EmployeeDTO dto = new EmployeeDTO
    {
      Name = name,
      Email = email
    };
    service.AddEmployee(dto);

  }
}