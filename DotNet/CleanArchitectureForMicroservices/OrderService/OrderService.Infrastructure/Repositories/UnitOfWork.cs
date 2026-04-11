using OrderService.Application.Interfaces.Repository;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services;

namespace OrderService.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing repositories in a single transaction.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
  private readonly OrderServiceDbContext _context;
  private ICartRepository? _cartRepository;
  private IOrderRepository? _orderRepository;
  private IInvoiceRepository? _invoiceRepository;

  public UnitOfWork(OrderServiceDbContext context)
  {
    _context = context;
  }

  /// <inheritdoc />
  public ICartRepository CartRepository
  {
    get => _cartRepository ??= new CartRepository(_context);
  }

  /// <inheritdoc />
  public IOrderRepository OrderRepository
  {
    get => _orderRepository ??= new OrderRepository(_context);
  }

  /// <inheritdoc />
  public IInvoiceRepository InvoiceRepository
  {
    get => _invoiceRepository ??= new InvoiceRepository(_context);
  }

  /// <inheritdoc />
  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await _context.SaveChangesAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
  {
    await _context.Database.BeginTransactionAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task CommitAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      await _context.SaveChangesAsync(cancellationToken);
      await _context.Database.CommitTransactionAsync(cancellationToken);
    }
    catch
    {
      await RollbackAsync(cancellationToken);
      throw;
    }
  }

  /// <inheritdoc />
  public async Task RollbackAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      await _context.Database.RollbackTransactionAsync(cancellationToken);
    }
    catch
    {
      // Rollback failed
    }
  }

  /// <inheritdoc />
  public async ValueTask DisposeAsync()
  {
    await _context.DisposeAsync();
  }
}
