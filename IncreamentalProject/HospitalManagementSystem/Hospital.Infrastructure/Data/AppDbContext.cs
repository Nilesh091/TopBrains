using System;
using Hospital.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                    .HasMany(d => d.Patients)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey(p => p.DoctorId);
        }
    }
}
