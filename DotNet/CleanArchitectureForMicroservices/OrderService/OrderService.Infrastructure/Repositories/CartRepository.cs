using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces.Repository;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services;

namespace OrderService.Infrastructure.Repositories;

/// <summary>
/// Cart repository implementation.
/// </summary>
public class CartRepository : Repository<Cart>, ICartRepository
{
  public CartRepository(OrderServiceDbContext context) : base(context)
  {
  }

  /// <inheritdoc />
  public async Task<Cart?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(c => c.Items)
        .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<bool> DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default)
  {
    var cart = await GetByUserIdAsync(userId, cancellationToken);
    if (cart == null)
      return false;

    DbSet.Remove(cart);
    return true;
  }
}
