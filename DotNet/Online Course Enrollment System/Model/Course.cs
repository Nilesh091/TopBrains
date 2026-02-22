using System;

namespace Online_Course_Enrollment_System.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
