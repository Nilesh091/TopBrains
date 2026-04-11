using OrderService.Application.DTOs.Invoice;
using OrderService.Application.Interfaces;
using OrderService.Application.Interfaces.Repository;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Services;

/// <summary>
/// Service for managing invoices.
/// </summary>
public class InvoiceService : IInvoiceService
{
  private readonly IUnitOfWork _unitOfWork;

  public InvoiceService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  /// <inheritdoc />
  public async Task<InvoiceDto> GenerateInvoiceAsync(Guid orderId, string paymentId, CancellationToken cancellationToken = default)
  {
    var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, cancellationToken);
    if (order == null)
      throw new InvalidOperationException("Order not found");

    if (order.PaymentStatus != Domain.Enums.PaymentStatus.Success)
      throw new InvalidOperationException("Order payment is not successful");

    // Check if invoice already exists
    var existingInvoice = await _unitOfWork.InvoiceRepository.GetByOrderIdAsync(orderId, cancellationToken);
    if (existingInvoice != null)
      return MapToDto(existingInvoice);

    // Create invoice
    var invoice = new Invoice
    {
      Id = Guid.NewGuid(),
      InvoiceNumber = GenerateInvoiceNumber(),
      OrderId = orderId,
      UserId = order.UserId,
      PaymentId = paymentId,
      SubTotal = order.TotalAmount,
      TaxAmount = 0m, // Calculate tax based on your business rules
      DiscountAmount = 0m, // Calculate discount if applicable
      TotalAmount = order.TotalAmount + 0m - 0m, // SubTotal + Tax - Discount
      IssuedAt = DateTime.UtcNow,
      PaidAt = DateTime.UtcNow,
      Status = InvoiceStatus.Paid,
      Notes = $"Invoice for Order {order.OrderNumber}"
    };

    await _unitOfWork.InvoiceRepository.AddAsync(invoice, cancellationToken);

    // Link invoice to order
    order.InvoiceId = invoice.Id;
    await _unitOfWork.OrderRepository.UpdateAsync(order, cancellationToken);

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return MapToDto(invoice);
  }

  /// <inheritdoc />
  public async Task<InvoiceDto?> GetInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default)
  {
    var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
    return invoice == null ? null : MapToDto(invoice);
  }

  /// <inheritdoc />
  public async Task<InvoiceDto?> GetInvoiceByNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
  {
    var invoice = await _unitOfWork.InvoiceRepository.GetByInvoiceNumberAsync(invoiceNumber, cancellationToken);
    return invoice == null ? null : MapToDto(invoice);
  }

  /// <inheritdoc />
  public async Task<List<InvoiceDto>> GetUserInvoicesAsync(string userId, CancellationToken cancellationToken = default)
  {
    var invoices = await _unitOfWork.InvoiceRepository.GetByUserIdAsync(userId, cancellationToken);
    return invoices.Select(MapToDto).ToList();
  }

  /// <inheritdoc />
  public async Task<InvoiceDto?> GetInvoiceByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
  {
    var invoice = await _unitOfWork.InvoiceRepository.GetByOrderIdAsync(orderId, cancellationToken);
    return invoice == null ? null : MapToDto(invoice);
  }

  /// <summary>
  /// Generates a unique invoice number.
  /// </summary>
  private string GenerateInvoiceNumber()
  {
    return $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
  }

  /// <summary>
  /// Maps Invoice entity to InvoiceDto.
  /// </summary>
  private InvoiceDto MapToDto(Invoice invoice)
  {
    return new InvoiceDto
    {
      Id = invoice.Id,
      InvoiceNumber = invoice.InvoiceNumber,
      OrderId = invoice.OrderId,
      UserId = invoice.UserId,
      SubTotal = invoice.SubTotal,
      TaxAmount = invoice.TaxAmount,
      DiscountAmount = invoice.DiscountAmount,
      TotalAmount = invoice.TotalAmount,
      PaymentId = invoice.PaymentId,
      Status = invoice.Status.ToString(),
      IssuedAt = invoice.IssuedAt,
      PaidAt = invoice.PaidAt,
      DueDate = invoice.DueDate,
      Notes = invoice.Notes
    };
  }
}
