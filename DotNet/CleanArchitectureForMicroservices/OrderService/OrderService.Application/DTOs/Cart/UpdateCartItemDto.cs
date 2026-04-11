namespace OrderService.Application.DTOs.Cart;

/// <summary>
/// DTO for updating cart item quantity.
/// </summary>
public class UpdateCartItemDto
{
  /// <summary>Cart item ID.</summary>
  public Guid CartItemId { get; set; }

  /// <summary>New quantity.</summary>
  public int Quantity { get; set; }
}
