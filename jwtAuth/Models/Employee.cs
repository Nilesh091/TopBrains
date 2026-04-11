using System;

namespace jwtAuth.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Company { get; set; }
        public string? Designation { get; set; }
    }
}
