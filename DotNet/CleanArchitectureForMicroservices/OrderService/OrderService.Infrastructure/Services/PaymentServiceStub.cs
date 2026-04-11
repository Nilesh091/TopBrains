using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs.Payment;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services;

/// <summary>
/// Placeholder implementation for Payment Service integration.
/// Replace this with actual Payment Service client implementation.
/// </summary>
public class PaymentServiceStub : IPaymentService
{
  private readonly ILogger<PaymentServiceStub> _logger;

  public PaymentServiceStub(ILogger<PaymentServiceStub> logger)
  {
    _logger = logger;
  }

  /// <inheritdoc />
  public async Task<PaymentResponseDto> InitiatePaymentAsync(InitiatePaymentDto initiatePaymentDto, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation($"[STUB] Initiating payment for Order {initiatePaymentDto.OrderId}");

    // Placeholder: Call actual Payment Service here
    // For now, returning a mock successful response
    return await Task.FromResult(new PaymentResponseDto
    {
      PaymentId = Guid.NewGuid().ToString(),
      Status = "Pending",
      Amount = initiatePaymentDto.Amount,
      PaymentMethod = "Card",
      Timestamp = DateTime.UtcNow,
      PaymentUrl = $"https://payment-service.local/pay?id={Guid.NewGuid()}"
    });
  }

  /// <inheritdoc />
  public async Task<PaymentResponseDto> VerifyPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation($"[STUB] Verifying payment {paymentId}");

    // Placeholder: Call actual Payment Service here
    return await Task.FromResult(new PaymentResponseDto
    {
      PaymentId = paymentId,
      Status = "Success",
      Amount = 0,
      Timestamp = DateTime.UtcNow
    });
  }

  /// <inheritdoc />
  public async Task<PaymentResponseDto> RefundPaymentAsync(string paymentId, decimal amount, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation($"[STUB] Refunding payment {paymentId} for amount {amount}");

    // Placeholder: Call actual Payment Service here
    return await Task.FromResult(new PaymentResponseDto
    {
      PaymentId = paymentId,
      Status = "Refunded",
      Amount = amount,
      Timestamp = DateTime.UtcNow
    });
  }
}
