using EmployeeManagement.Models.Context;
using Microsoft.EntityFrameworkCore;
class Program
{
  public static void Main()
  {
    var context = new Context();
    var pfAmount = context.Pfs
      .Where(p => p.Employee.Name == "Nilesh")
      .Select(p => p.PfAmount)
      .FirstOrDefault();

    Console.WriteLine($"PF Amount of Nilesh: {pfAmount}");

    var highestPfEmployee = context.Pfs.Include(p => p.Employee).OrderByDescending(p => p.PfAmount).FirstOrDefault(); ;
    Console.WriteLine($"Highest PF Holder: {highestPfEmployee.Employee.Name}");


  }
}