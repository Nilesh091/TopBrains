using System;
using Online_Course_Enrollment_System.Model;
using Online_Course_Enrollment_System.Model.Context;

namespace Online_Course_Enrollment_System.Services
{
    public class CourseManagement : ICourseManagementService
    {
        private readonly Connection connection;

        public CourseManagement(Connection c)
        {
            connection = c;
        }
        public List<Course> GetAll()
        {
            return connection.Courses.ToList();
        }
        public bool UpdateCourse(int id, Course course)
        {
            var existingCourse = connection.Courses.Find(id);
            if (existingCourse == null) return false;

            existingCourse.Description = course.Description;
            existingCourse.Duration = course.Duration;
            existingCourse.Title = course.Title;
            connection.SaveChanges();
            return true;
        }
        public void AddCourse(Course course)
        {
            connection.Courses.Add(course);
            connection.SaveChanges();
        }
        public bool DeleteCourse(int id)
        {
            var existingCourse = connection.Courses.Find(id);

            if (existingCourse == null)
                return false;

            connection.Courses.Remove(existingCourse);
            connection.SaveChanges();

            return true;
        }
    }
}
