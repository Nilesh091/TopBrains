using OrderService.Application.DTOs.Payment;

namespace OrderService.Application.Interfaces;

/// <summary>
/// Interface for payment service integration.
/// </summary>
public interface IPaymentService
{
  /// <summary>
  /// Initiates payment for an order.
  /// </summary>
  Task<PaymentResponseDto> InitiatePaymentAsync(InitiatePaymentDto initiatePaymentDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Verifies payment status from Payment Service.
  /// </summary>
  Task<PaymentResponseDto> VerifyPaymentAsync(string paymentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Refunds a payment.
  /// </summary>
  Task<PaymentResponseDto> RefundPaymentAsync(string paymentId, decimal amount, CancellationToken cancellationToken = default);
}
