using OrderService.Domain.Enums;

namespace OrderService.Application.DTOs.Order;

/// <summary>
/// DTO for order response.
/// </summary>
public class OrderDto
{
  /// <summary>Order ID.</summary>
  public Guid Id { get; set; }

  /// <summary>Order number.</summary>
  public string OrderNumber { get; set; } = null!;

  /// <summary>User ID.</summary>
  public string UserId { get; set; } = null!;

  /// <summary>Collection of order items.</summary>
  public List<OrderItemDto> Items { get; set; } = new();

  /// <summary>Total amount.</summary>
  public decimal TotalAmount { get; set; }

  /// <summary>Current order status.</summary>
  public OrderStatus Status { get; set; }

  /// <summary>Payment status.</summary>
  public PaymentStatus PaymentStatus { get; set; }

  /// <summary>Payment ID (if paid).</summary>
  public string? PaymentId { get; set; }

  /// <summary>Shipping address.</summary>
  public string? ShippingAddress { get; set; }

  /// <summary>Invoice ID (if invoice generated).</summary>
  public Guid? InvoiceId { get; set; }

  /// <summary>Date when the order was created.</summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>Date when the order was last updated.</summary>
  public DateTime UpdatedAt { get; set; }
}
