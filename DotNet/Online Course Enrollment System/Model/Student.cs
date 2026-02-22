using System;

namespace Online_Course_Enrollment_System.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
