using System;
using Online_Course_Enrollment_System.Model;
using Online_Course_Enrollment_System.Model.Context;

namespace Online_Course_Enrollment_System.Services
{
    public class Enrollment_Management : IEnrollmentManagementService
    {
        private readonly Connection context;

        public Enrollment_Management(Connection connection)
        {
            this.context = connection;
        }
        public Enrollment EnrollStudentInCourse(Enrollment enr)
        {
            var exist = context.Enrollments.Any(e => e.CourseId == enr.CourseId && e.StrudentId == enr.StrudentId);
            if (exist)
            {
                return null;
            }
            Enrollment enrollment = new Enrollment();
            enrollment.StrudentId = enr.StrudentId;
            enrollment.CourseId = enr.CourseId;
            context.Enrollments.Add(enrollment);
            context.SaveChanges();
            return enrollment;
        }
        public List<Enrollment> GetAllEnrollments()
        {
            return context.Enrollments.ToList();

        }
        public List<Course> GetAllEnroledCourse(int studentId)
        {
            return context.Enrollments.Where(e => e.StrudentId == studentId).Select(e => e.Course).ToList();
        }
        public List<Student> GetAllStudentsEnrolled(int courseId)
        {
            return context.Enrollments.Where(e => e.CourseId == courseId).Select(e => e.Student).ToList();
        }
    }
}
