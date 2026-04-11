namespace OrderService.Domain.Entities;

/// <summary>
/// Represents an individual item in an order.
/// </summary>
public class OrderItem
{
  /// <summary>Unique identifier for the order item.</summary>
  public Guid Id { get; set; }

  /// <summary>Reference to the parent order.</summary>
  public Guid OrderId { get; set; }

  /// <summary>Product ID from the Product Service.</summary>
  public string ProductId { get; set; } = null!;

  /// <summary>Product name for record-keeping.</summary>
  public string ProductName { get; set; } = null!;

  /// <summary>Unit price of the product at the time of order.</summary>
  public decimal UnitPrice { get; set; }

  /// <summary>Quantity ordered.</summary>
  public int Quantity { get; set; }

  /// <summary>Navigation property to the parent order.</summary>
  public Order? Order { get; set; }

  /// <summary>Calculates the line total (UnitPrice * Quantity).</summary>
  public decimal GetLineTotal()
  {
    return UnitPrice * Quantity;
  }
}
