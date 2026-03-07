using System;

namespace EmployeeManagement.Models
{
    public class Pf
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int PfAmount { get; set; }
        public Employee Employee { get; set; }
    }

}
