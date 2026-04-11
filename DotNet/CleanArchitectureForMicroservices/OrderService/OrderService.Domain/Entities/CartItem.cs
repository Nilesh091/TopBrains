namespace OrderService.Domain.Entities;

/// <summary>
/// Represents an individual item in a cart.
/// </summary>
public class CartItem
{
  /// <summary>Unique identifier for the cart item.</summary>
  public Guid Id { get; set; }

  /// <summary>Reference to the parent cart.</summary>
  public Guid CartId { get; set; }

  /// <summary>Product ID from the Product Service.</summary>
  public string ProductId { get; set; } = null!;

  /// <summary>Product name for display purposes.</summary>
  public string ProductName { get; set; } = null!;

  /// <summary>Price of the product at the time it was added to cart.</summary>
  public decimal Price { get; set; }

  /// <summary>Quantity of the product in the cart.</summary>
  public int Quantity { get; set; }

  /// <summary>Date when the item was added to the cart.</summary>
  public DateTime AddedAt { get; set; }

  /// <summary>Navigation property to the parent cart.</summary>
  public Cart? Cart { get; set; }

  /// <summary>Calculates the line total (Price * Quantity).</summary>
  public decimal GetLineTotal()
  {
    return Price * Quantity;
  }
}
