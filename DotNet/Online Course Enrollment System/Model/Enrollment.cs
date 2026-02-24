using System;

namespace Online_Course_Enrollment_System.Model
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StrudentId { get; set; }

        public Student? Student { get; set; }
        public Course? Course { get; set; }


    }
}
