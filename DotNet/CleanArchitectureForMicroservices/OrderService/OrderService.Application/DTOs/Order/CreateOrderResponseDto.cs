namespace OrderService.Application.DTOs.Order;

/// <summary>
/// DTO for order creation response with payment details.
/// </summary>
public class CreateOrderResponseDto
{
  /// <summary>Order ID.</summary>
  public Guid OrderId { get; set; }

  /// <summary>Order number.</summary>
  public string OrderNumber { get; set; } = null!;

  /// <summary>Total amount to be paid.</summary>
  public decimal TotalAmount { get; set; }

  /// <summary>Payment URL or ID (from Payment Service).</summary>
  public string? PaymentUrl { get; set; }

  /// <summary>Message with order and payment details.</summary>
  public string Message { get; set; } = null!;
}
