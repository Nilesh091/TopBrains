namespace OrderService.Domain.Entities;

/// <summary>
/// Represents a customer's shopping cart.
/// </summary>
public class Cart
{
  /// <summary>Unique identifier for the cart.</summary>
  public Guid Id { get; set; }

  /// <summary>User ID (derived from JWT token).</summary>
  public string UserId { get; set; } = null!;

  /// <summary>Collection of items in the cart.</summary>
  public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

  /// <summary>Date when the cart was created.</summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>Date when the cart was last updated.</summary>
  public DateTime UpdatedAt { get; set; }

  /// <summary>Calculates the total price of all items in the cart.</summary>
  public decimal GetTotal()
  {
    return Items.Sum(item => item.Price * item.Quantity);
  }

  /// <summary>Clears all items from the cart.</summary>
  public void Clear()
  {
    Items.Clear();
    UpdatedAt = DateTime.UtcNow;
  }
}
