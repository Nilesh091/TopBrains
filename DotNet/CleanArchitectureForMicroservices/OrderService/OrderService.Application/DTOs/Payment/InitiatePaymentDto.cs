namespace OrderService.Application.DTOs.Payment;

/// <summary>
/// DTO for payment initiation request to Payment Service.
/// </summary>
public class InitiatePaymentDto
{
  /// <summary>Order ID.</summary>
  public Guid OrderId { get; set; }

  /// <summary>Amount to be paid.</summary>
  public decimal Amount { get; set; }

  /// <summary>Currency code (e.g., USD, INR).</summary>
  public string Currency { get; set; } = "USD";

  /// <summary>Description of the payment.</summary>
  public string Description { get; set; } = null!;

  /// <summary>User ID for payment tracking.</summary>
  public string UserId { get; set; } = null!;

  /// <summary>Success callback URL.</summary>
  public string? SuccessCallbackUrl { get; set; }

  /// <summary>Failure callback URL.</summary>
  public string? FailureCallbackUrl { get; set; }
}
