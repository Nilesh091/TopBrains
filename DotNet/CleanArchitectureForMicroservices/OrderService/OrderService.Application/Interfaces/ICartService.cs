using OrderService.Application.DTOs.Cart;

namespace OrderService.Application.Interfaces;

/// <summary>
/// Interface for cart management operations.
/// </summary>
public interface ICartService
{
  /// <summary>
  /// Gets the cart for a user.
  /// </summary>
  Task<CartDto?> GetCartAsync(string userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds an item to the cart.
  /// </summary>
  Task<CartDto> AddToCartAsync(string userId, AddToCartDto addToCartDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes an item from the cart.
  /// </summary>
  Task<CartDto> RemoveFromCartAsync(string userId, Guid cartItemId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates the quantity of a cart item.
  /// </summary>
  Task<CartDto> UpdateCartItemAsync(string userId, UpdateCartItemDto updateCartItemDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Clears all items from the cart.
  /// </summary>
  Task ClearCartAsync(string userId, CancellationToken cancellationToken = default);
}
