using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
  public class OrderDbContext : DbContext
  {
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Order>().HasData(
          new Order { Id = 1, UserId = 1, ProductName = "Laptop", Quantity = 1, TotalPrice = 1200.99m, OrderDate = new DateTime(2024, 06, 01, 10, 0, 0) },
          new Order { Id = 2, UserId = 1, ProductName = "Mouse", Quantity = 2, TotalPrice = 40.50m, OrderDate = new DateTime(2024, 06, 01, 10, 0, 0) },
          new Order { Id = 3, UserId = 2, ProductName = "Keyboard", Quantity = 1, TotalPrice = 70.00m, OrderDate = new DateTime(2024, 06, 01, 10, 0, 0) },
          new Order { Id = 4, UserId = 2, ProductName = "Monitor", Quantity = 1, TotalPrice = 250.00m, OrderDate = new DateTime(2024, 06, 01, 10, 0, 0) },
          new Order { Id = 5, UserId = 1, ProductName = "USB Cable", Quantity = 3, TotalPrice = 15.75m, OrderDate = new DateTime(2024, 06, 01, 10, 0, 0) }
      );
    }
  }
}
