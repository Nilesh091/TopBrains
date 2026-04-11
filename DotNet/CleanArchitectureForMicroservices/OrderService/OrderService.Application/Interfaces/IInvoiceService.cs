using OrderService.Application.DTOs.Invoice;

namespace OrderService.Application.Interfaces;

/// <summary>
/// Interface for invoice management operations.
/// </summary>
public interface IInvoiceService
{
  /// <summary>
  /// Generates an invoice for a paid order.
  /// </summary>
  Task<InvoiceDto> GenerateInvoiceAsync(Guid orderId, string paymentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets an invoice by ID.
  /// </summary>
  Task<InvoiceDto?> GetInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets an invoice by invoice number.
  /// </summary>
  Task<InvoiceDto?> GetInvoiceByNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets all invoices for a user.
  /// </summary>
  Task<List<InvoiceDto>> GetUserInvoicesAsync(string userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets an invoice by order ID.
  /// </summary>
  Task<InvoiceDto?> GetInvoiceByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
