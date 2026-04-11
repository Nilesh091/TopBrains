using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for Order Service.
/// </summary>
public class OrderServiceDbContext : DbContext
{
  public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options) : base(options)
  {
  }

  /// <summary>Cart entities.</summary>
  public DbSet<Cart> Carts { get; set; }

  /// <summary>Cart items.</summary>
  public DbSet<CartItem> CartItems { get; set; }

  /// <summary>Order entities.</summary>
  public DbSet<Order> Orders { get; set; }

  /// <summary>Order items.</summary>
  public DbSet<OrderItem> OrderItems { get; set; }

  /// <summary>Invoice entities.</summary>
  public DbSet<Invoice> Invoices { get; set; }

  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure Cart entity
    modelBuilder.Entity<Cart>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.UserId).IsRequired().HasMaxLength(100);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasMany(e => e.Items)
              .WithOne(e => e.Cart)
              .HasForeignKey(e => e.CartId)
              .OnDelete(DeleteBehavior.Cascade);

      entity.HasIndex(e => e.UserId).IsUnique().HasName("IX_Cart_UserId");
    });

    // Configure CartItem entity
    modelBuilder.Entity<CartItem>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.ProductId).IsRequired().HasMaxLength(100);
      entity.Property(e => e.ProductName).IsRequired().HasMaxLength(500);
      entity.Property(e => e.Price).HasPrecision(18, 2);
      entity.Property(e => e.Quantity).IsRequired();
      entity.Property(e => e.AddedAt).IsRequired();

      entity.HasIndex(e => e.CartId).HasName("IX_CartItem_CartId");
    });

    // Configure Order entity
    modelBuilder.Entity<Order>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
      entity.Property(e => e.UserId).IsRequired().HasMaxLength(100);
      entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
      entity.Property(e => e.Status).IsRequired();
      entity.Property(e => e.PaymentStatus).IsRequired();
      entity.Property(e => e.PaymentId).HasMaxLength(100);
      entity.Property(e => e.ShippingAddress).HasMaxLength(1000);
      entity.Property(e => e.CreatedAt).IsRequired();
      entity.Property(e => e.UpdatedAt).IsRequired();

      entity.HasMany(e => e.Items)
              .WithOne(e => e.Order)
              .HasForeignKey(e => e.OrderId)
              .OnDelete(DeleteBehavior.Cascade);

      entity.HasOne(e => e.Invoice)
              .WithOne(e => e.Order)
              .HasForeignKey<Invoice>(e => e.OrderId)
              .OnDelete(DeleteBehavior.Cascade);

      entity.HasIndex(e => e.OrderNumber).IsUnique().HasName("IX_Order_OrderNumber");
      entity.HasIndex(e => e.UserId).HasName("IX_Order_UserId");
    });

    // Configure OrderItem entity
    modelBuilder.Entity<OrderItem>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.ProductId).IsRequired().HasMaxLength(100);
      entity.Property(e => e.ProductName).IsRequired().HasMaxLength(500);
      entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
      entity.Property(e => e.Quantity).IsRequired();

      entity.HasIndex(e => e.OrderId).HasName("IX_OrderItem_OrderId");
    });

    // Configure Invoice entity
    modelBuilder.Entity<Invoice>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
      entity.Property(e => e.UserId).IsRequired().HasMaxLength(100);
      entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
      entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
      entity.Property(e => e.SubTotal).HasPrecision(18, 2);
      entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
      entity.Property(e => e.PaymentId).HasMaxLength(100);
      entity.Property(e => e.IssuedAt).IsRequired();
      entity.Property(e => e.Status).IsRequired();
      entity.Property(e => e.Notes).HasMaxLength(2000);

      entity.HasIndex(e => e.InvoiceNumber).IsUnique().HasName("IX_Invoice_InvoiceNumber");
      entity.HasIndex(e => e.UserId).HasName("IX_Invoice_UserId");
      entity.HasIndex(e => e.OrderId).HasName("IX_Invoice_OrderId");
    });
  }
}
