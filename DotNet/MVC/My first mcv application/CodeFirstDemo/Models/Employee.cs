using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CodeFirstDemo.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        [DisplayName("Employee Name")]
        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100, MinimumLength = 3)]
        public string EmpName { get; set; }
        //address and salary with required and string length 300
        [Required(ErrorMessage = "Employee address is required.")]
        [StringLength(300, MinimumLength = 5)]
        public string EmpAddress { get; set; }
        [Required(ErrorMessage = "Employee salary is required.")]
        [Range(3000, 100000, ErrorMessage = "Salary must be between 3000 and 100000.")]
        public decimal Salary { get; set; }
        //email
        [Required(ErrorMessage = "Employee email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public int? DepartmentDeptId { get; set; }
        public Department? Department { get; set; }
    }
}
