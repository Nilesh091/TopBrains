using System;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
       .HasOne(c => c.Department)
       .WithMany(d => d.Courses)
       .HasForeignKey(c => c.DepartmentId)
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    DepartmentId = 1,
                    DepartmentName = "Computer Science",
                    Description = "Department of Computer Science"
                },
                new Department
                {
                    DepartmentId = 2,
                    DepartmentName = "Management",
                    Description = "Department of Business Management"
                }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseId = 1,
                    CourseName = "BCA",
                    Duration = "3 Years",
                    Fees = 60000,
                    DepartmentId = 1
                },
                new Course
                {
                    CourseId = 2,
                    CourseName = "MCA",
                    Duration = "2 Years",
                    Fees = 90000,
                    DepartmentId = 1
                },
                new Course
                {
                    CourseId = 3,
                    CourseName = "MBA",
                    Duration = "2 Years",
                    Fees = 120000,
                    DepartmentId = 2
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "Admin Teacher",
                    Email = "teacher@test.com",
                    Password = "123456",
                    Role = "Teacher"
                },
                new User
                {
                    UserId = 2,
                    FullName = "Test Student",
                    Email = "student@test.com",
                    Password = "123456",
                    Role = "Student"
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentId = 1,
                    StudentName = "Test Student",
                    Email = "student@test.com",
                    PhoneNumber = "9876543210",
                    Address = "Punjab",
                    DepartmentId = 1,
                    CourseId = 1
                }
            );
        }
    }
}
