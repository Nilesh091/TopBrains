using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        [StringLength(100)]
        public string CourseName { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Duration { get; set; } = null!;
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
