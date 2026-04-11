namespace OrderService.Application.DTOs.Cart;

/// <summary>
/// DTO for cart item response.
/// </summary>
public class CartItemDto
{
  /// <summary>Cart item ID.</summary>
  public Guid Id { get; set; }

  /// <summary>Product ID.</summary>
  public string ProductId { get; set; } = null!;

  /// <summary>Product name.</summary>
  public string ProductName { get; set; } = null!;

  /// <summary>Price of the product.</summary>
  public decimal Price { get; set; }

  /// <summary>Quantity in cart.</summary>
  public int Quantity { get; set; }

  /// <summary>Line total (Price * Quantity).</summary>
  public decimal LineTotal { get; set; }

  /// <summary>Date when added to cart.</summary>
  public DateTime AddedAt { get; set; }
}
