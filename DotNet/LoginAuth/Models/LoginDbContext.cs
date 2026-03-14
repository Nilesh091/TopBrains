using System;

using Microsoft.EntityFrameworkCore;

namespace LoginAuth.Models
{
    public class LoginDbContext : DbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options)
        {

        }
        public DbSet<UserLogin> UserLogins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<UserLogin>().HasData(
                new UserLogin { Id = 1, Username = "admin", Password = "admin123", IsActive = 1 },
                new UserLogin { Id = 2, Username = "user1", Password = "user123", IsActive = 1 },
                new UserLogin { Id = 3, Username = "user2", Password = "user123", IsActive = 1 }
            );
        }
    }
}
