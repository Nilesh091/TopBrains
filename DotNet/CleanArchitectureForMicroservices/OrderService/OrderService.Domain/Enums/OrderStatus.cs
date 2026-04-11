namespace OrderService.Domain.Enums;

/// <summary>
/// Represents the status of an order throughout its lifecycle.
/// </summary>
public enum OrderStatus
{
  /// <summary>Order is created but payment is pending.</summary>
  Pending = 1,

  /// <summary>Payment has been successfully processed.</summary>
  Paid = 2,

  /// <summary>Payment failed or order was cancelled.</summary>
  Failed = 3,

  /// <summary>Order has been shipped.</summary>
  Shipped = 4,

  /// <summary>Order has been delivered to the customer.</summary>
  Delivered = 5,

  /// <summary>Order has been cancelled.</summary>
  Cancelled = 6
}
