namespace OrderService.Application.DTOs.Cart;

/// <summary>
/// DTO for adding an item to the cart.
/// </summary>
public class AddToCartDto
{
  /// <summary>Product ID from the Product Service.</summary>
  public required string ProductId { get; set; }

  /// <summary>Product name.</summary>
  public required string ProductName { get; set; }

  /// <summary>Unit price of the product.</summary>
  public decimal Price { get; set; }

  /// <summary>Quantity to add to the cart.</summary>
  public int Quantity { get; set; }
}
