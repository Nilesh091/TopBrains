using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public Pf Pf { get; set; }
    }

}
