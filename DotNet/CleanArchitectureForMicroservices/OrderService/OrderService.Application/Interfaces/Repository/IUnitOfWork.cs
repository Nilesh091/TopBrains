namespace OrderService.Application.Interfaces.Repository;

/// <summary>
/// Unit of Work pattern interface for managing multiple repositories in a single transaction.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
  /// <summary>Repository for Cart entities.</summary>
  ICartRepository CartRepository { get; }

  /// <summary>Repository for Order entities.</summary>
  IOrderRepository OrderRepository { get; }

  /// <summary>Repository for Invoice entities.</summary>
  IInvoiceRepository InvoiceRepository { get; }

  /// <summary>Saves all changes to the database.</summary>
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

  /// <summary>Begins a database transaction.</summary>
  Task BeginTransactionAsync(CancellationToken cancellationToken = default);

  /// <summary>Commits the current transaction.</summary>
  Task CommitAsync(CancellationToken cancellationToken = default);

  /// <summary>Rolls back the current transaction.</summary>
  Task RollbackAsync(CancellationToken cancellationToken = default);
}
