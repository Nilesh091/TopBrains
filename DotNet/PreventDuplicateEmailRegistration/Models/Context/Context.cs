using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace PreventDuplicateEmailRegistration.Models.Context
{
    public class Context : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Duplicate_email_detection;User Id=sa;Password=2004@Nilu;TrustServerCertificate=True;");
        }

    }
}
