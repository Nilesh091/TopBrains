using OrderService.Application.DTOs.Cart;
using OrderService.Application.Interfaces;
using OrderService.Application.Interfaces.Repository;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

/// <summary>
/// Service for managing shopping carts.
/// </summary>
public class CartService : ICartService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IProductServiceClient _productServiceClient;

  public CartService(IUnitOfWork unitOfWork, IProductServiceClient productServiceClient)
  {
    _unitOfWork = unitOfWork;
    _productServiceClient = productServiceClient;
  }

  /// <inheritdoc />
  public async Task<CartDto?> GetCartAsync(string userId, CancellationToken cancellationToken = default)
  {
    var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    if (cart == null)
      return null;

    return MapToDto(cart);
  }

  /// <inheritdoc />
  public async Task<CartDto> AddToCartAsync(string userId, AddToCartDto addToCartDto, CancellationToken cancellationToken = default)
  {
    // Get or create cart for user
    var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);

    if (cart == null)
    {
      // Create new cart with the first item
      cart = new Cart
      {
        // Id is store-generated (ValueGeneratedOnAdd). Leaving it default allows EF to treat this as a new entity.
        Id = default,
        UserId = userId,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Items = new List<CartItem>()
      };

      var firstItem = new CartItem
      {
        // Id is store-generated (ValueGeneratedOnAdd).
        Id = default,
        CartId = cart.Id,
        ProductId = addToCartDto.ProductId,
        ProductName = addToCartDto.ProductName,
        Price = addToCartDto.Price,
        Quantity = addToCartDto.Quantity,
        AddedAt = DateTime.UtcNow
      };

      cart.Items.Add(firstItem);
      await _unitOfWork.CartRepository.AddAsync(cart, cancellationToken);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    else
    {
      // Add or update item in existing cart
      var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == addToCartDto.ProductId);

      if (existingItem != null)
      {
        // Item exists, just update quantity
        existingItem.Quantity += addToCartDto.Quantity;
      }
      else
      {
        // Create new CartItem
        var newItem = new CartItem
        {
          // Id is store-generated (ValueGeneratedOnAdd). Leaving it default ensures EF treats this as Added.
          Id = default,
          CartId = cart.Id,
          ProductId = addToCartDto.ProductId,
          ProductName = addToCartDto.ProductName,
          Price = addToCartDto.Price,
          Quantity = addToCartDto.Quantity,
          AddedAt = DateTime.UtcNow
        };
        cart.Items.Add(newItem);
      }

      cart.UpdatedAt = DateTime.UtcNow;

      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    // Reload cart from database to get fresh state
    var updatedCart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    return MapToDto(updatedCart!);
  }

  /// <inheritdoc />
  public async Task<CartDto> RemoveFromCartAsync(string userId, Guid cartItemId, CancellationToken cancellationToken = default)
  {
    var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    if (cart == null)
      throw new InvalidOperationException("Cart not found for user");

    var item = cart.Items.FirstOrDefault(x => x.Id == cartItemId);
    if (item == null)
      throw new InvalidOperationException("Item not found in cart");

    cart.Items.Remove(item);
    cart.UpdatedAt = DateTime.UtcNow;
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Reload cart from database to get fresh state
    var updatedCart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    return MapToDto(updatedCart!);
  }

  /// <inheritdoc />
  public async Task<CartDto> UpdateCartItemAsync(string userId, UpdateCartItemDto updateCartItemDto, CancellationToken cancellationToken = default)
  {
    var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    if (cart == null)
      throw new InvalidOperationException("Cart not found for user");

    var item = cart.Items.FirstOrDefault(x => x.Id == updateCartItemDto.CartItemId);
    if (item == null)
      throw new InvalidOperationException("Item not found in cart");

    if (updateCartItemDto.Quantity <= 0)
      cart.Items.Remove(item);
    else
      item.Quantity = updateCartItemDto.Quantity;

    cart.UpdatedAt = DateTime.UtcNow;
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    // Reload cart from database to get fresh state
    var updatedCart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    return MapToDto(updatedCart!);
  }

  /// <inheritdoc />
  public async Task ClearCartAsync(string userId, CancellationToken cancellationToken = default)
  {
    var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId, cancellationToken);
    if (cart != null)
    {
      cart.Clear();
      await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
  }

  /// <summary>
  /// Maps Cart entity to CartDto.
  /// </summary>
  private CartDto MapToDto(Cart cart)
  {
    return new CartDto
    {
      Id = cart.Id,
      UserId = cart.UserId,
      Items = cart.Items.Select(item => new CartItemDto
      {
        Id = item.Id,
        ProductId = item.ProductId,
        ProductName = item.ProductName,
        Price = item.Price,
        Quantity = item.Quantity,
        LineTotal = item.GetLineTotal(),
        AddedAt = item.AddedAt
      }).ToList(),
      Total = cart.GetTotal(),
      ItemCount = cart.Items.Count,
      CreatedAt = cart.CreatedAt,
      UpdatedAt = cart.UpdatedAt
    };
  }
}
