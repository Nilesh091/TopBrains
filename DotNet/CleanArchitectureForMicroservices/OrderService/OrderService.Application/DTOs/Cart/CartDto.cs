namespace OrderService.Application.DTOs.Cart;

/// <summary>
/// DTO for cart response.
/// </summary>
public class CartDto
{
  /// <summary>Cart ID.</summary>
  public Guid Id { get; set; }

  /// <summary>User ID.</summary>
  public string UserId { get; set; } = null!;

  /// <summary>Collection of items in the cart.</summary>
  public List<CartItemDto> Items { get; set; } = new();

  /// <summary>Total amount in the cart.</summary>
  public decimal Total { get; set; }

  /// <summary>Count of items in the cart.</summary>
  public int ItemCount { get; set; }

  /// <summary>Date when the cart was created.</summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>Date when the cart was last updated.</summary>
  public DateTime UpdatedAt { get; set; }
}
