using OrderService.Application.DTOs.Order;
using OrderService.Application.Interfaces;
using OrderService.Application.Interfaces.Repository;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Services;

/// <summary>
/// Service for managing orders.
/// </summary>
public class OrderService : IOrderService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IProductServiceClient _productServiceClient;
  private readonly IPaymentService _paymentService;

  public OrderService(IUnitOfWork unitOfWork, IProductServiceClient productServiceClient, IPaymentService paymentService)
  {
    _unitOfWork = unitOfWork;
    _productServiceClient = productServiceClient;
    _paymentService = paymentService;
  }

  /// <inheritdoc />
  public async Task<CreateOrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto, CancellationToken cancellationToken = default)
  {
    // Get user's cart
    var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    if (cart == null || !cart.Items.Any())
      throw new InvalidOperationException("Cart is empty");

    // Validate product availability
    foreach (var cartItem in cart.Items)
    {
      var isInStock = await _productServiceClient.CheckStockAsync(cartItem.ProductId, cartItem.Quantity, cancellationToken);
      if (!isInStock)
        throw new InvalidOperationException($"Product {cartItem.ProductName} is not in stock");
    }

    // Create order
    var order = new Order
    {
      Id = Guid.NewGuid(),
      OrderNumber = GenerateOrderNumber(),
      UserId = userId,
      ShippingAddress = createOrderDto.ShippingAddress,
      Status = OrderStatus.Pending,
      PaymentStatus = PaymentStatus.Pending,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };

    // Add order items from cart
    foreach (var cartItem in cart.Items)
    {
      var orderItem = new OrderItem
      {
        Id = Guid.NewGuid(),
        OrderId = order.Id,
        ProductId = cartItem.ProductId,
        ProductName = cartItem.ProductName,
        UnitPrice = cartItem.Price,
        Quantity = cartItem.Quantity
      };
      order.Items.Add(orderItem);
    }

    order.TotalAmount = order.CalculateTotal();

    await _unitOfWork.OrderRepository.AddAsync(order, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Initiate payment
    var paymentDto = new DTOs.Payment.InitiatePaymentDto
    {
      OrderId = order.Id,
      Amount = order.TotalAmount,
      Currency = "USD",
      Description = $"Order {order.OrderNumber}",
      UserId = userId
    };

    var paymentResponse = await _paymentService.InitiatePaymentAsync(paymentDto, cancellationToken);

    // Clear user's cart
    cart.Clear();
    await _unitOfWork.CartRepository.UpdateAsync(cart, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return new CreateOrderResponseDto
    {
      OrderId = order.Id,
      OrderNumber = order.OrderNumber,
      TotalAmount = order.TotalAmount,
      PaymentUrl = paymentResponse.PaymentUrl,
      Message = $"Order created successfully. Please proceed to payment."
    };
  }

  /// <inheritdoc />
  public async Task<OrderDto?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
  {
    var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, cancellationToken);
    return order == null ? null : MapToDto(order);
  }

  /// <inheritdoc />
  public async Task<List<OrderDto>> GetUserOrdersAsync(string userId, CancellationToken cancellationToken = default)
  {
    var orders = await _unitOfWork.OrderRepository.GetByUserIdAsync(userId, cancellationToken);
    return orders.Select(MapToDto).ToList();
  }

  /// <inheritdoc />
  public async Task<OrderDto?> GetOrderByNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
  {
    var order = await _unitOfWork.OrderRepository.GetByOrderNumberAsync(orderNumber, cancellationToken);
    return order == null ? null : MapToDto(order);
  }

  /// <inheritdoc />
  public async Task<OrderDto> ConfirmPaymentAsync(Guid orderId, string paymentId, CancellationToken cancellationToken = default)
  {
    var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, cancellationToken);
    if (order == null)
      throw new InvalidOperationException("Order not found");

    if (!order.CanBePaid())
      throw new InvalidOperationException("Order cannot be paid in its current state");

    // Verify payment with Payment Service
    var verifyPayment = await _paymentService.VerifyPaymentAsync(paymentId, cancellationToken);
    if (verifyPayment.Status != "Success")
      throw new InvalidOperationException("Payment verification failed");

    order.MarkAsPaid(paymentId);
    await _unitOfWork.OrderRepository.UpdateAsync(order, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return MapToDto(order);
  }

  /// <inheritdoc />
  public async Task<OrderDto> MarkOrderPaymentFailedAsync(Guid orderId, string reason, CancellationToken cancellationToken = default)
  {
    var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, cancellationToken);
    if (order == null)
      throw new InvalidOperationException("Order not found");

    order.MarkAsFailed(reason);
    await _unitOfWork.OrderRepository.UpdateAsync(order, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return MapToDto(order);
  }

  /// <summary>
  /// Generates a unique order number.
  /// </summary>
  private string GenerateOrderNumber()
  {
    return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
  }

  /// <summary>
  /// Maps Order entity to OrderDto.
  /// </summary>
  private OrderDto MapToDto(Order order)
  {
    return new OrderDto
    {
      Id = order.Id,
      OrderNumber = order.OrderNumber,
      UserId = order.UserId,
      Items = order.Items.Select(item => new OrderItemDto
      {
        Id = item.Id,
        ProductId = item.ProductId,
        ProductName = item.ProductName,
        UnitPrice = item.UnitPrice,
        Quantity = item.Quantity,
        LineTotal = item.GetLineTotal()
      }).ToList(),
      TotalAmount = order.TotalAmount,
      Status = order.Status,
      PaymentStatus = order.PaymentStatus,
      PaymentId = order.PaymentId,
      ShippingAddress = order.ShippingAddress,
      InvoiceId = order.InvoiceId,
      CreatedAt = order.CreatedAt,
      UpdatedAt = order.UpdatedAt
    };
  }
}
