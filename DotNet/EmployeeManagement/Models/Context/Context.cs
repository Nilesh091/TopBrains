using System;
using Microsoft.EntityFrameworkCore;
namespace EmployeeManagement.Models.Context
{
    public class Context : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Pf> Pfs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=EmpManagement;User Id=sa;Password=2004@Nilu;TrustServerCertificate=True;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
            .HasOne(e => e.Pf)
            .WithOne(p => p.Employee)
            .HasForeignKey<Pf>(p => p.EmpId);

            modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "Nilesh", Salary = 50000 },
            new Employee { Id = 2, Name = "Rahul", Salary = 60000 }
        );
            modelBuilder.Entity<Pf>().HasData(
                new Pf { Id = 1, EmpId = 1, PfAmount = 5000 },
                new Pf { Id = 2, EmpId = 2, PfAmount = 6000 }
            );
        }
    }
}
