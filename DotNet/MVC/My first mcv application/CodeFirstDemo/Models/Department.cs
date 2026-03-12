using System;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstDemo.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
        [Required]
        [StringLength(100)]
        public string DeptName { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
