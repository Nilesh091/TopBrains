using System;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Models;
namespace Student_Management_System.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
    .HasOne(s => s.Department)
    .WithMany(d => d.Students)
    .HasForeignKey(s => s.DepartmentId);
            modelBuilder.Entity<Course>().HasOne(s => s.Department).WithMany(s => s.Courses).HasForeignKey(s => s.DepartmentId);

            modelBuilder.Entity<Department>().HasData(
    new Department { DepartmentId = 1, DepartmentName = "Computer Science", Location = "Building A" },
    new Department { DepartmentId = 2, DepartmentName = "Information Technology", Location = "Building B" },
    new Department { DepartmentId = 3, DepartmentName = "Business Administration", Location = "Building C" }
);
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Ravi Kumar", Email = "ravi@gmail.com", Age = 22, Gender = "Male", DepartmentId = 1, CourseId = 1 },
                new Student { StudentId = 2, Name = "Anjali Sharma", Email = "anjali@gmail.com", Age = 23, Gender = "Female", DepartmentId = 2, CourseId = 3 },
                new Student { StudentId = 3, Name = "Suresh Reddy", Email = "suresh@gmail.com", Age = 24, Gender = "Male", DepartmentId = 1, CourseId = 2 },
                new Student { StudentId = 4, Name = "Priya Nair", Email = "priya@gmail.com", Age = 21, Gender = "Female", DepartmentId = 3, CourseId = 5 }
            );
            modelBuilder.Entity<Course>().HasData(
        new Course { CourseId = 1, CourseName = ".NET Full Stack Development", Duration = "6 Months", DepartmentId = 1 },
        new Course { CourseId = 2, CourseName = "Angular Development", Duration = "4 Months", DepartmentId = 1 },
        new Course { CourseId = 3, CourseName = "Cloud Computing", Duration = "6 Months", DepartmentId = 2 },
        new Course { CourseId = 4, CourseName = "Cyber Security", Duration = "5 Months", DepartmentId = 2 },
        new Course { CourseId = 5, CourseName = "Financial Management", Duration = "3 Months", DepartmentId = 3 }
    );


        }

    }

}
