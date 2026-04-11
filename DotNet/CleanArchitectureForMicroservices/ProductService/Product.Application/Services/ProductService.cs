using Product.Application.DTOs;
using Product.Application.Interfaces;
using Product.Domain.Entities;
using Product.Domain.Interfaces;

namespace Product.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponseDto<List<ProductDto>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _repository.GetAllAsync();
            var productDtos = products
                .Select(p => MapToDto(p))
                .ToList();

            return new ApiResponseDto<List<ProductDto>>
            {
                Success = true,
                Data = productDtos,
                Message = "Products retrieved successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<List<ProductDto>>
            {
                Success = false,
                Data = null,
                Message = $"Error retrieving products: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponseDto<ProductDto>> GetProductByIdAsync(Guid id)
    {
        try
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return new ApiResponseDto<ProductDto>
                {
                    Success = false,
                    Data = null,
                    Message = "Product not found"
                };
            }

            return new ApiResponseDto<ProductDto>
            {
                Success = true,
                Data = MapToDto(product),
                Message = "Product retrieved successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<ProductDto>
            {
                Success = false,
                Data = null,
                Message = $"Error retrieving product: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponseDto<ProductDto>> CreateProductAsync(CreateProductDto dto)
    {
        try
        {
            var product = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdProduct = await _repository.CreateAsync(product);

            return new ApiResponseDto<ProductDto>
            {
                Success = true,
                Data = MapToDto(createdProduct),
                Message = "Product created successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<ProductDto>
            {
                Success = false,
                Data = null,
                Message = $"Error creating product: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponseDto<ProductDto>> UpdateProductAsync(UpdateProductDto dto)
    {
        try
        {
            var exists = await _repository.ProductExistsAsync(dto.Id);

            if (!exists)
            {
                return new ApiResponseDto<ProductDto>
                {
                    Success = false,
                    Data = null,
                    Message = "Product not found"
                };
            }

            var product = new ProductEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category,
                UpdatedAt = DateTime.UtcNow
            };

            var updatedProduct = await _repository.UpdateAsync(product);

            return new ApiResponseDto<ProductDto>
            {
                Success = true,
                Data = MapToDto(updatedProduct),
                Message = "Product updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<ProductDto>
            {
                Success = false,
                Data = null,
                Message = $"Error updating product: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponseDto<bool>> DeleteProductAsync(Guid id)
    {
        try
        {
            var exists = await _repository.ProductExistsAsync(id);

            if (!exists)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Product not found"
                };
            }

            var result = await _repository.DeleteAsync(id);

            return new ApiResponseDto<bool>
            {
                Success = result,
                Data = result,
                Message = result ? "Product deleted successfully" : "Failed to delete product"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<bool>
            {
                Success = false,
                Data = false,
                Message = $"Error deleting product: {ex.Message}"
            };
        }
    }

    private ProductDto MapToDto(ProductEntity product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}
