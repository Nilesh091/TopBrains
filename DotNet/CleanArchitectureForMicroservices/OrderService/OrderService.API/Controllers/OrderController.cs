using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs.Common;
using OrderService.Application.DTOs.Invoice;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Interfaces;
using System.Security.Claims;

namespace OrderService.API.Controllers;

/// <summary>
/// API controller for order operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Buyer")]
public class OrderController : ControllerBase
{
  private readonly IOrderService _orderService;
  private readonly IInvoiceService _invoiceService;
  private readonly ILogger<OrderController> _logger;

  public OrderController(IOrderService orderService, IInvoiceService invoiceService, ILogger<OrderController> logger)
  {
    _orderService = orderService;
    _invoiceService = invoiceService;
    _logger = logger;
  }

  /// <summary>
  /// Creates an order from the user's cart.
  /// </summary>
  /// <param name="createOrderDto">Order creation details including shipping address.</param>
  /// <returns>Created order with payment URL.</returns>
  [HttpPost("create")]
  [ProducesResponseType(typeof(ApiResponse<CreateOrderResponseDto>), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto, CancellationToken cancellationToken)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ApiResponse.ErrorResponse("Invalid request data", "VALIDATION_ERROR"));
      }

      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId))
      {
        return Unauthorized(ApiResponse.ErrorResponse("User ID not found in token", "INVALID_TOKEN"));
      }

      var response = await _orderService.CreateOrderAsync(userId, createOrderDto, cancellationToken);
      return CreatedAtAction(nameof(GetOrder), new { orderId = response.OrderId },
          ApiResponse<CreateOrderResponseDto>.SuccessResponse(response, "Order created successfully"));
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(ApiResponse.ErrorResponse(ex.Message, "INVALID_OPERATION"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error creating order: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse("An error occurred while creating order", "ERROR"));
    }
  }

  /// <summary>
  /// Gets a specific order by ID.
  /// </summary>
  /// <param name="orderId">The order ID.</param>
  /// <returns>Order details.</returns>
  [HttpGet("{orderId}")]
  [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetOrder(Guid orderId, CancellationToken cancellationToken)
  {
    try
    {
      var order = await _orderService.GetOrderAsync(orderId, cancellationToken);
      if (order == null)
      {
        return NotFound(ApiResponse<OrderDto>.ErrorResponse("Order not found", "ORDER_NOT_FOUND"));
      }

      return Ok(ApiResponse<OrderDto>.SuccessResponse(order, "Order retrieved successfully"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error getting order: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse("An error occurred while retrieving order", "ERROR"));
    }
  }

  /// <summary>
  /// Gets all orders for the current user.
  /// </summary>
  /// <returns>List of user's orders.</returns>
  [HttpGet("user/all")]
  [ProducesResponseType(typeof(ApiResponse<List<OrderDto>>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetUserOrders(CancellationToken cancellationToken)
  {
    try
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId))
      {
        return Unauthorized(ApiResponse.ErrorResponse("User ID not found in token", "INVALID_TOKEN"));
      }

      var orders = await _orderService.GetUserOrdersAsync(userId, cancellationToken);
      return Ok(ApiResponse<List<OrderDto>>.SuccessResponse(orders, "Orders retrieved successfully"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error getting user orders: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse("An error occurred while retrieving orders", "ERROR"));
    }
  }

  /// <summary>
  /// Confirms payment for an order.
  /// </summary>
  /// <param name="orderId">The order ID.</param>
  /// <param name="paymentId">The payment ID from Payment Service.</param>
  /// <returns>Updated order with paid status.</returns>
  [HttpPost("{orderId}/confirm-payment")]
  [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> ConfirmPayment(Guid orderId, [FromQuery] string paymentId, CancellationToken cancellationToken)
  {
    try
    {
      if (string.IsNullOrEmpty(paymentId))
      {
        return BadRequest(ApiResponse.ErrorResponse("Payment ID is required", "VALIDATION_ERROR"));
      }

      var order = await _orderService.ConfirmPaymentAsync(orderId, paymentId, cancellationToken);

      // Generate invoice after successful payment
      var invoice = await _invoiceService.GenerateInvoiceAsync(orderId, paymentId, cancellationToken);

      return Ok(ApiResponse<OrderDto>.SuccessResponse(order,
          "Payment confirmed and invoice generated successfully"));
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(ApiResponse.ErrorResponse(ex.Message, "INVALID_OPERATION"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error confirming payment: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse("An error occurred while confirming payment", "ERROR"));
    }
  }

  /// <summary>
  /// Gets invoice for an order.
  /// </summary>
  /// <param name="orderId">The order ID.</param>
  /// <returns>Invoice details.</returns>
  [HttpGet("{orderId}/invoice")]
  [ProducesResponseType(typeof(ApiResponse<InvoiceDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetOrderInvoice(Guid orderId, CancellationToken cancellationToken)
  {
    try
    {
      var invoice = await _invoiceService.GetInvoiceByOrderIdAsync(orderId, cancellationToken);
      if (invoice == null)
      {
        return NotFound(ApiResponse<InvoiceDto>.ErrorResponse("Invoice not found", "INVOICE_NOT_FOUND"));
      }

      return Ok(ApiResponse<InvoiceDto>.SuccessResponse(invoice, "Invoice retrieved successfully"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error getting invoice: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse("An error occurred while retrieving invoice", "ERROR"));
    }
  }

  /// <summary>
  /// Gets all invoices for the current user.
  /// </summary>
  /// <returns>List of user's invoices.</returns>
  [HttpGet("invoices/all")]
  [ProducesResponseType(typeof(ApiResponse<List<InvoiceDto>>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetUserInvoices(CancellationToken cancellationToken)
  {
    try
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId))
      {
        return Unauthorized(ApiResponse.ErrorResponse("User ID not found in token", "INVALID_TOKEN"));
      }

      var invoices = await _invoiceService.GetUserInvoicesAsync(userId, cancellationToken);
      return Ok(ApiResponse<List<InvoiceDto>>.SuccessResponse(invoices, "Invoices retrieved successfully"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error getting user invoices: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse("An error occurred while retrieving invoices", "ERROR"));
    }
  }
}
