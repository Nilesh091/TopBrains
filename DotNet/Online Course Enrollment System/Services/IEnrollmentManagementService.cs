using System;
using Online_Course_Enrollment_System.Model;

namespace Online_Course_Enrollment_System.Services
{
    public interface IEnrollmentManagementService
    {
        Enrollment EnrollStudentInCourse(Enrollment enr);
        List<Enrollment> GetAllEnrollments();
        List<Course> GetAllEnroledCourse(int studentId);
        List<Student> GetAllStudentsEnrolled(int courseId);
    }
}
