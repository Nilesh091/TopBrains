namespace OrderService.Domain.Enums;

/// <summary>
/// Represents the payment processing status.
/// </summary>
public enum PaymentStatus
{
  /// <summary>Payment is pending</summary>
  Pending = 1,

  /// <summary>Payment has been processed successfully</summary>
  Success = 2,

  /// <summary>Payment processing failed</summary>
  Failed = 3,

  /// <summary>Payment has been refunded</summary>
  Refunded = 4
}
