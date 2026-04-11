using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs.Cart;
using OrderService.Application.DTOs.Common;
using OrderService.Application.Interfaces;
using System.Security.Claims;

namespace OrderService.API.Controllers;

/// <summary>
/// API controller for cart operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Buyer")]
public class CartController : ControllerBase
{
  private readonly ICartService _cartService;
  private readonly ILogger<CartController> _logger;

  public CartController(ICartService cartService, ILogger<CartController> logger)
  {
    _cartService = cartService;
    _logger = logger;
  }

  /// <summary>
  /// Gets the current user's cart.
  /// </summary>
  /// <returns>Cart details with items.</returns>
  [HttpGet]
  [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
  {
    try
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId))
      {
        return Unauthorized(ApiResponse.ErrorResponse("User ID not found in token", "INVALID_TOKEN"));
      }

      var cart = await _cartService.GetCartAsync(userId, cancellationToken);
      if (cart == null)
      {
        return NotFound(ApiResponse<CartDto>.ErrorResponse("Cart not found", "CART_NOT_FOUND"));
      }

      return Ok(ApiResponse<CartDto>.SuccessResponse(cart, "Cart retrieved successfully"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error getting cart: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse("An error occurred while retrieving cart", "ERROR"));
    }
  }

  /// <summary>
  /// Adds a product to the cart.
  /// </summary>
  /// <param name="addToCartDto">Product details to add.</param>
  /// <returns>Updated cart.</returns>
  [HttpPost("add")]
  [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto, CancellationToken cancellationToken)
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

      var cart = await _cartService.AddToCartAsync(userId, addToCartDto, cancellationToken);
      return Ok(ApiResponse<CartDto>.SuccessResponse(cart, "Product added to cart successfully"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error adding to cart: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse(ex.Message, "ERROR"));
    }
  }

  /// <summary>
  /// Updates the quantity of an item in the cart.
  /// </summary>
  /// <param name="updateCartItemDto">Item update details.</param>
  /// <returns>Updated cart.</returns>
  [HttpPut("update")]
  [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto, CancellationToken cancellationToken)
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

      var cart = await _cartService.UpdateCartItemAsync(userId, updateCartItemDto, cancellationToken);
      return Ok(ApiResponse<CartDto>.SuccessResponse(cart, "Cart item updated successfully"));
    }
    catch (InvalidOperationException ex)
    {
      return NotFound(ApiResponse.ErrorResponse(ex.Message, "ITEM_NOT_FOUND"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error updating cart item: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse(ex.Message, "ERROR"));
    }
  }

  /// <summary>
  /// Removes a product from the cart.
  /// </summary>
  /// <param name="cartItemId">ID of the cart item to remove.</param>
  /// <returns>Updated cart.</returns>
  [HttpDelete("remove/{cartItemId}")]
  [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> RemoveFromCart(Guid cartItemId, CancellationToken cancellationToken)
  {
    try
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId))
      {
        return Unauthorized(ApiResponse.ErrorResponse("User ID not found in token", "INVALID_TOKEN"));
      }

      var cart = await _cartService.RemoveFromCartAsync(userId, cartItemId, cancellationToken);
      return Ok(ApiResponse<CartDto>.SuccessResponse(cart, "Item removed from cart successfully"));
    }
    catch (InvalidOperationException ex)
    {
      return NotFound(ApiResponse.ErrorResponse(ex.Message, "ITEM_NOT_FOUND"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error removing from cart: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse(ex.Message, "ERROR"));
    }
  }

  /// <summary>
  /// Clears all items from the cart.
  /// </summary>
  [HttpDelete("clear")]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
  {
    try
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId))
      {
        return Unauthorized(ApiResponse.ErrorResponse("User ID not found in token", "INVALID_TOKEN"));
      }

      await _cartService.ClearCartAsync(userId, cancellationToken);
      return Ok(ApiResponse.SuccessResponse("Cart cleared successfully"));
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error clearing cart: {ex.Message}");
      return BadRequest(ApiResponse.ErrorResponse(ex.Message, "ERROR"));
    }
  }
}
