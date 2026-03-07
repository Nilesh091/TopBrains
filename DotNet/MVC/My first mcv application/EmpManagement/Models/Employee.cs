using System;

namespace EmpManagement.Models
{
    public class Employee
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string Department { get; set; } = string.Empty;
        public char Gender { get; set; }
    }
}
