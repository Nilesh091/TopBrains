using System;
using Student_Management_System.Models;
using Student_Management_System.Repositories;

namespace Student_Management_System.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Student> Students { get; }
        public IGenericRepository<Department> Departments { get; }
        public IGenericRepository<Course> Courses { get; }
        void Save();
    }
}
