using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces.Repository;

/// <summary>
/// Repository interface for Cart entity-specific operations.
/// </summary>
public interface ICartRepository : IRepository<Cart>
{
  /// <summary>Finds a cart by user ID.</summary>
  Task<Cart?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

  /// <summary>Deletes a cart by user ID.</summary>
  Task<bool> DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
