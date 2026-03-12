using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Department
    {

        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        [AllowedValues("HR", "IT", "Finance")]
        public string DepartmentName { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Location { get; set; } = null!;
        public IEnumerable<Student>? Students { get; set; }
        public IEnumerable<Course>? Courses { get; set; }

    }
}
