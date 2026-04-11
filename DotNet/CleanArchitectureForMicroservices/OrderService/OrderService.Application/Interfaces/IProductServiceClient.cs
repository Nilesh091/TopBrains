namespace OrderService.Application.Interfaces;

/// <summary>
/// Interface for external Product Service API calls.
/// </summary>
public interface IProductServiceClient
{
  /// <summary>
  /// Gets product details from Product Service.
  /// </summary>
  Task<ProductDto?> GetProductAsync(string productId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Verifies if products are in stock.
  /// </summary>
  Task<bool> CheckStockAsync(string productId, int quantity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets multiple products at once.
  /// </summary>
  Task<List<ProductDto>> GetProductsAsync(List<string> productIds, CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for product information from Product Service.
/// </summary>
public class ProductDto
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public decimal Price { get; set; }
  public int Stock { get; set; }
  public string? Description { get; set; }
}
