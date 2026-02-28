using System;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Model.Context
{
    public class Context : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=OrderManagementDB;User Id=sa;Password=2004@Nilu;TrustServerCertificate=True;");
        }
    }
}
