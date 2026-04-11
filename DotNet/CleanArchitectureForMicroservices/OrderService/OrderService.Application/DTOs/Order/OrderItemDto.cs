namespace OrderService.Application.DTOs.Order;

/// <summary>
/// DTO for order item response.
/// </summary>
public class OrderItemDto
{
  /// <summary>Order item ID.</summary>
  public Guid Id { get; set; }

  /// <summary>Product ID.</summary>
  public string ProductId { get; set; } = null!;

  /// <summary>Product name.</summary>
  public string ProductName { get; set; } = null!;

  /// <summary>Unit price at the time of order.</summary>
  public decimal UnitPrice { get; set; }

  /// <summary>Quantity ordered.</summary>
  public int Quantity { get; set; }

  /// <summary>Line total.</summary>
  public decimal LineTotal { get; set; }
}
