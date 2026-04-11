namespace OrderService.Application.DTOs.Invoice;

/// <summary>
/// DTO for invoice response.
/// </summary>
public class InvoiceDto
{
  /// <summary>Invoice ID.</summary>
  public Guid Id { get; set; }

  /// <summary>Invoice number.</summary>
  public string InvoiceNumber { get; set; } = null!;

  /// <summary>Associated order ID.</summary>
  public Guid OrderId { get; set; }

  /// <summary>User ID.</summary>
  public string UserId { get; set; } = null!;

  /// <summary>Subtotal before tax and discounts.</summary>
  public decimal SubTotal { get; set; }

  /// <summary>Tax amount.</summary>
  public decimal TaxAmount { get; set; }

  /// <summary>Discount amount.</summary>
  public decimal DiscountAmount { get; set; }

  /// <summary>Total amount.</summary>
  public decimal TotalAmount { get; set; }

  /// <summary>Payment ID from Payment Service.</summary>
  public string PaymentId { get; set; } = null!;

  /// <summary>Invoice status.</summary>
  public string Status { get; set; } = null!;

  /// <summary>Date when the invoice was issued.</summary>
  public DateTime IssuedAt { get; set; }

  /// <summary>Date when the payment was made.</summary>
  public DateTime? PaidAt { get; set; }

  /// <summary>Due date for payment.</summary>
  public DateTime? DueDate { get; set; }

  /// <summary>Notes.</summary>
  public string? Notes { get; set; }
}
