using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Student
    {

        [Key]
        public int StudentId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string? Email { get; set; }
        public string? Gender { get; set; }
        [Range(18, 60)]
        public int Age { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }

    }
}
