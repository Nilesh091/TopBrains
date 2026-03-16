using System;
using System.ComponentModel.DataAnnotations;
namespace StudentManagementSystem.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        public string Duration { get; set; }

        public decimal Fees { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Student> Students { get; set; }

    }
}
