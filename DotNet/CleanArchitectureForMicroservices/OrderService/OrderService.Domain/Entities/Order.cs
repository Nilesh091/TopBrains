using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

/// <summary>
/// Represents a customer order.
/// </summary>
public class Order
{
  /// <summary>Unique identifier for the order.</summary>
  public Guid Id { get; set; }

  /// <summary>Order number for display (e.g., ORD-2024-001).</summary>
  public string OrderNumber { get; set; } = null!;

  /// <summary>User ID (derived from JWT token).</summary>
  public string UserId { get; set; } = null!;

  /// <summary>Collection of items in the order.</summary>
  public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

  /// <summary>Total amount for the order.</summary>
  public decimal TotalAmount { get; set; }

  /// <summary>Current status of the order.</summary>
  public OrderStatus Status { get; set; }

  /// <summary>Payment status of the order.</summary>
  public PaymentStatus PaymentStatus { get; set; }

  /// <summary>Payment ID from the Payment Service (if payment processed).</summary>
  public string? PaymentId { get; set; }

  /// <summary>Shipping address.</summary>
  public string? ShippingAddress { get; set; }

  /// <summary>Reference to the invoice generated after successful payment.</summary>
  public Guid? InvoiceId { get; set; }

  /// <summary>Date when the order was created.</summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>Date when the order was last updated.</summary>
  public DateTime UpdatedAt { get; set; }

  /// <summary>Date when the order was completed (null if not yet completed).</summary>
  public DateTime? CompletedAt { get; set; }

  /// <summary>Navigation property to the associated invoice.</summary>
  public Invoice? Invoice { get; set; }

  /// <summary>Checks if the order can be paid.</summary>
  public bool CanBePaid()
  {
    return Status == OrderStatus.Pending && PaymentStatus == PaymentStatus.Pending;
  }

  /// <summary>Marks the order as paid and updates the status.</summary>
  public void MarkAsPaid(string paymentId)
  {
    PaymentId = paymentId;
    PaymentStatus = PaymentStatus.Success;
    Status = OrderStatus.Paid;
    UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>Marks the order as failed.</summary>
  public void MarkAsFailed(string reason)
  {
    PaymentStatus = PaymentStatus.Failed;
    Status = OrderStatus.Failed;
    UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>Calculates the total amount from order items.</summary>
  public decimal CalculateTotal()
  {
    return Items.Sum(item => item.UnitPrice * item.Quantity);
  }
}
