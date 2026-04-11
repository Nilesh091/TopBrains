using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces.Repository;

/// <summary>
/// Repository interface for Invoice entity-specific operations.
/// </summary>
public interface IInvoiceRepository : IRepository<Invoice>
{
  /// <summary>Finds an invoice by invoice number.</summary>
  Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);

  /// <summary>Gets all invoices for a specific user.</summary>
  Task<IEnumerable<Invoice>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

  /// <summary>Gets an invoice by order ID.</summary>
  Task<Invoice?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
