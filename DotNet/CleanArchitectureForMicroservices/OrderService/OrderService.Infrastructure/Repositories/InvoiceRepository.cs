using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces.Repository;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services;

namespace OrderService.Infrastructure.Repositories;

/// <summary>
/// Invoice repository implementation.
/// </summary>
public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
  public InvoiceRepository(OrderServiceDbContext context) : base(context)
  {
  }

  /// <inheritdoc />
  public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(i => i.Order)
        .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IEnumerable<Invoice>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(i => i.Order)
        .Where(i => i.UserId == userId)
        .OrderByDescending(i => i.IssuedAt)
        .ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<Invoice?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(i => i.Order)
        .FirstOrDefaultAsync(i => i.OrderId == orderId, cancellationToken);
  }

  /// <inheritdoc />
  public override async Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    return await DbSet
        .Include(i => i.Order)
        .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
  }
}
