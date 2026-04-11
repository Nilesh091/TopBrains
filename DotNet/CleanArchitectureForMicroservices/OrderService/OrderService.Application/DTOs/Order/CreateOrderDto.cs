namespace OrderService.Application.DTOs.Order;

/// <summary>
/// DTO for creating an order from cart.
/// </summary>
public class CreateOrderDto
{
  /// <summary>Shipping address for the order.</summary>
  public required string ShippingAddress { get; set; }

  /// <summary>Additional notes for the order (optional).</summary>
  public string? Notes { get; set; }
}
