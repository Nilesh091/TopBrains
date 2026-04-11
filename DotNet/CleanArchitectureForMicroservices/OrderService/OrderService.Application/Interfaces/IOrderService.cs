using OrderService.Application.DTOs.Order;

namespace OrderService.Application.Interfaces;

/// <summary>
/// Interface for order management operations.
/// </summary>
public interface IOrderService
{
  /// <summary>
  /// Creates an order from the user's cart.
  /// </summary>
  Task<CreateOrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets an order by ID.
  /// </summary>
  Task<OrderDto?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets all orders for a user.
  /// </summary>
  Task<List<OrderDto>> GetUserOrdersAsync(string userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets an order by order number.
  /// </summary>
  Task<OrderDto?> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

  /// <summary>
  /// Confirms payment for an order.
  /// </summary>
  Task<OrderDto> ConfirmPaymentAsync(Guid orderId, string paymentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Marks an order payment as failed.
  /// </summary>
  Task<OrderDto> MarkOrderPaymentFailedAsync(Guid orderId, string reason, CancellationToken cancellationToken = default);
}
