namespace OrderService.Domain.Entities;

/// <summary>
/// Represents an invoice generated after successful payment.
/// </summary>
public class Invoice
{
  /// <summary>Unique identifier for the invoice.</summary>
  public Guid Id { get; set; }

  /// <summary>Invoice number (e.g., INV-2024-001).</summary>
  public string InvoiceNumber { get; set; } = null!;

  /// <summary>Reference to the associated order.</summary>
  public Guid OrderId { get; set; }

  /// <summary>Navigation property to the associated order.</summary>
  public Order? Order { get; set; }

  /// <summary>User ID (derived from JWT token).</summary>
  public string UserId { get; set; } = null!;

  /// <summary>Total amount of the invoice.</summary>
  public decimal TotalAmount { get; set; }

  /// <summary>Tax amount (if calculated).</summary>
  public decimal TaxAmount { get; set; }

  /// <summary>Subtotal before tax.</summary>
  public decimal SubTotal { get; set; }

  /// <summary>Discount amount (if applicable).</summary>
  public decimal DiscountAmount { get; set; }

  /// <summary>Payment ID from the Payment Service.</summary>
  public string PaymentId { get; set; } = null!;

  /// <summary>Invoice issue date.</summary>
  public DateTime IssuedAt { get; set; }

  /// <summary>Due date for payment (if applicable).</summary>
  public DateTime? DueDate { get; set; }

  /// <summary>Payment date.</summary>
  public DateTime? PaidAt { get; set; }

  /// <summary>Notes or additional information.</summary>
  public string? Notes { get; set; }

  /// <summary>Invoice status (e.g., Draft, Issued, Paid, Cancelled).</summary>
  public InvoiceStatus Status { get; set; }
}

/// <summary>
/// Represents the status of an invoice.
/// </summary>
public enum InvoiceStatus
{
  /// <summary>Invoice is in draft state.</summary>
  Draft = 1,

  /// <summary>Invoice has been issued.</summary>
  Issued = 2,

  /// <summary>Invoice has been paid.</summary>
  Paid = 3,

  /// <summary>Invoice has been cancelled.</summary>
  Cancelled = 4,

  /// <summary>Invoice is overdue.</summary>
  Overdue = 5
}
