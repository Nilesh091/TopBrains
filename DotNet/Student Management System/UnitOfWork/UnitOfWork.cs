using System;
using Student_Management_System.Context;
using Student_Management_System.Models;
using Student_Management_System.Repositories;

namespace Student_Management_System.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IGenericRepository<Student> Students { get; private set; }
        public IGenericRepository<Department> Departments { get; private set; }
        public IGenericRepository<Course> Courses { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Students = new GenericRepository<Student>(_context);
            Departments = new GenericRepository<Department>(_context);
            Courses = new GenericRepository<Course>(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
