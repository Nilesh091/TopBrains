using System;
using Online_Course_Enrollment_System.Model;

namespace Online_Course_Enrollment_System.Services
{
    public interface IStudentManagementService
    {
        Student AddStudent(Student student);
        Student UpdateStudentDetails(int id, Student student);
        Student DeleteStudent(int id);
        List<Student> GetAll();
    }
}
