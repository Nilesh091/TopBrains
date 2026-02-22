using System;
using Microsoft.EntityFrameworkCore;

namespace Online_Course_Enrollment_System.Model.Context
{
    public class Connection : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>().HasOne(s => s.Course).WithMany(s => s.Enrollments).HasForeignKey(s => s.StrudentId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Enrollment>().HasOne(s => s.Student).WithMany(s => s.Enrollments).HasForeignKey(s => s.CourseId).OnDelete(DeleteBehavior.Cascade);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Online_Course_Enrollment_System;User Id=sa;Password=2004@Nilu;TrustServerCertificate=True;");
        }
    }
}
