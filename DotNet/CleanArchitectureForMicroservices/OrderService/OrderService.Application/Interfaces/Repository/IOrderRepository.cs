using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces.Repository;

/// <summary>
/// Repository interface for Order entity-specific operations.
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
  /// <summary>Finds an order by order number.</summary>
  Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

  /// <summary>Gets all orders for a specific user.</summary>
  Task<IEnumerable<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

  /// <summary>Gets orders by status.</summary>
  Task<IEnumerable<Order>> GetByStatusAsync(Domain.Enums.OrderStatus status, CancellationToken cancellationToken = default);
}
