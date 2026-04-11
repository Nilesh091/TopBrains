using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
  public class UserDbContext : DbContext
  {
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().HasData(
          new User { Id = 1, Name = "Alice", Email = "alice@example.com", PasswordHash = "hashedpassword1" },
          new User { Id = 2, Name = "Bob", Email = "bob@example.com", PasswordHash = "hashedpassword2" },
          new User { Id = 3, Name = "Charlie", Email = "charlie@example.com", PasswordHash = "hashedpassword3" },
          new User { Id = 4, Name = "David", Email = "david@example.com", PasswordHash = "hashedpassword4" },
          new User { Id = 5, Name = "Eve", Email = "eve@example.com", PasswordHash = "hashedpassword5" },
          new User { Id = 6, Name = "Frank", Email = "frank@example.com", PasswordHash = "hashedpassword6" },
          new User { Id = 7, Name = "Grace", Email = "grace@example.com", PasswordHash = "hashedpassword7" },
          new User { Id = 8, Name = "Hank", Email = "hank@example.com", PasswordHash = "hashedpassword8" },
          new User { Id = 9, Name = "Ivy", Email = "ivy@example.com", PasswordHash = "hashedpassword9" },
          new User { Id = 10, Name = "Jack", Email = "jack@example.com", PasswordHash = "hashedpassword10" }
      );
    }
  }
}
