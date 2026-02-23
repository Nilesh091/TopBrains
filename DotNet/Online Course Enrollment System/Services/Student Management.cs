using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata;
using Online_Course_Enrollment_System.Model;
using Online_Course_Enrollment_System.Model.Context;

namespace Online_Course_Enrollment_System.Services
{
    public class Student_Management : IStudentManagementService
    {
        private readonly Connection context;
        public Student_Management(Connection con)
        {
            this.context = con;
        }
        public Student AddStudent(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return student;
        }

        public Student UpdateStudentDetails(int id, Student student)
        {
            var studentToBeUpdated = context.Students.Find(id);
            if (studentToBeUpdated == null)
            {
                return null;
            }
            studentToBeUpdated.Email = student.Email;
            studentToBeUpdated.Name = student.Name;
            return studentToBeUpdated;
        }

        public Student DeleteStudent(int id)
        {
            var studentToBeDeleted = context.Students.Find(id);
            if (studentToBeDeleted == null)
            {
                return null;
            }
            context.Students.Remove(studentToBeDeleted);
            return studentToBeDeleted;
        }
        public List<Student> GetAll()
        {
            return context.Students.ToList();
        }
    }
}
