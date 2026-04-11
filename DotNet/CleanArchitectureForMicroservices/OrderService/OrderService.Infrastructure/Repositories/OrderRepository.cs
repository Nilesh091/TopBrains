using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces.Repository;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services;

namespace OrderService.Infrastructure.Repositories;

/// <summary>
/// Order repository implementation.
/// </summary>
public class OrderRepository : Repository<Order>, IOrderRepository
{
  public OrderRepository(OrderServiceDbContext context) : base(context)
  {
  }

  /// <inheritdoc />
  public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(o => o.Items)
        .Include(o => o.Invoice)
        .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(o => o.Items)
        .Include(o => o.Invoice)
        .Where(o => o.UserId == userId)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(o => o.Items)
        .Where(o => o.Status == status)
        .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(o => o.Items)
        .Include(o => o.Invoice)
        .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
  }
}
