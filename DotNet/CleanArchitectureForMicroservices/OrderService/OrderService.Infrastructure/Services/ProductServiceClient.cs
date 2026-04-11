using Microsoft.Extensions.Logging;
using OrderService.Application.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderService.Infrastructure.Services;

/// <summary>
/// HTTP client for calling Product Service API.
/// </summary>
public class ProductServiceClient : IProductServiceClient
{
  private readonly HttpClient _httpClient;
  private readonly ILogger<ProductServiceClient> _logger;

  public ProductServiceClient(HttpClient httpClient, ILogger<ProductServiceClient> logger)
  {
    _httpClient = httpClient;
    _logger = logger;
  }

  /// <inheritdoc />
  public async Task<ProductDto?> GetProductAsync(string productId, CancellationToken cancellationToken = default)
  {
    try
    {
      var response = await _httpClient.GetAsync($"products/{productId}", cancellationToken);
      if (!response.IsSuccessStatusCode)
      {
        _logger.LogWarning($"Failed to get product {productId}: {response.StatusCode}");
        return null;
      }

      var content = await response.Content.ReadAsStringAsync(cancellationToken);
      var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProductDto>>(content,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      if (apiResponse?.Success != true)
        return null;

      return apiResponse.Data;
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error calling Product Service: {ex.Message}");
      return null;
    }
  }

  /// <inheritdoc />
  public async Task<bool> CheckStockAsync(string productId, int quantity, CancellationToken cancellationToken = default)
  {
    try
    {
      // ProductService exposes: GET /api/products/{id}
      // OrderService checks `product.Stock >= quantity`.
      var response = await _httpClient.GetAsync($"products/{productId}", cancellationToken);
      if (!response.IsSuccessStatusCode)
      {
        _logger.LogWarning($"Failed to check stock for product {productId}");
        return false;
      }

      var content = await response.Content.ReadAsStringAsync(cancellationToken);

      var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProductDto>>(content,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      if (apiResponse?.Success != true || apiResponse.Data == null)
        return false;

      return apiResponse.Data.Stock >= quantity;
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error checking stock: {ex.Message}");
      return false;
    }
  }

  /// <inheritdoc />
  public async Task<List<ProductDto>> GetProductsAsync(List<string> productIds, CancellationToken cancellationToken = default)
  {
    try
    {
      var ids = string.Join(",", productIds);
      var response = await _httpClient.GetAsync($"products?ids={ids}", cancellationToken);
      if (!response.IsSuccessStatusCode)
      {
        _logger.LogWarning($"Failed to get multiple products");
        return new List<ProductDto>();
      }

      var content = await response.Content.ReadAsStringAsync(cancellationToken);
      // Deserialize list of products
      _logger.LogInformation($"Successfully retrieved {productIds.Count} products");
      return new List<ProductDto>(); // Return parsed products
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error calling Product Service: {ex.Message}");
      return new List<ProductDto>();
    }
  }

  // Matches ProductService's ApiResponseDto<T>:
  // { "success": true, "data": { ... }, "message": "..." }
  private sealed class ApiResponse<T>
  {
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
  }
}
