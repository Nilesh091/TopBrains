using System;
using Online_Course_Enrollment_System.Model;

namespace Online_Course_Enrollment_System.Services
{
    public interface ICourseManagementService
    {
        List<Course> GetAll();
        void AddCourse(Course course);
        bool UpdateCourse(int id, Course course);
        bool DeleteCourse(int id);

    }
}
