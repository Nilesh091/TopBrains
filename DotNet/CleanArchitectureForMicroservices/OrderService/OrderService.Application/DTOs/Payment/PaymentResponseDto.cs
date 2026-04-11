namespace OrderService.Application.DTOs.Payment;

/// <summary>
/// DTO for payment response from Payment Service.
/// </summary>
public class PaymentResponseDto
{
  /// <summary>Payment ID.</summary>
  public string PaymentId { get; set; } = null!;

  /// <summary>Payment status.</summary>
  public string Status { get; set; } = null!;

  /// <summary>Amount paid.</summary>
  public decimal Amount { get; set; }

  /// <summary>Payment method used.</summary>
  public string? PaymentMethod { get; set; }

  /// <summary>Transaction ID from payment gateway.</summary>
  public string? TransactionId { get; set; }

  /// <summary>Timestamp of the payment.</summary>
  public DateTime Timestamp { get; set; }

  /// <summary>Payment URL (for redirecting customer).</summary>
  public string? PaymentUrl { get; set; }
}
