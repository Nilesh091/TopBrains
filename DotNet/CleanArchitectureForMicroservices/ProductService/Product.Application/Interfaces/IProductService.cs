using Product.Application.DTOs;

namespace Product.Application.Interfaces;

public interface IProductService
{
    Task<ApiResponseDto<List<ProductDto>>> GetAllProductsAsync();
    Task<ApiResponseDto<ProductDto>> GetProductByIdAsync(Guid id);
    Task<ApiResponseDto<ProductDto>> CreateProductAsync(CreateProductDto dto);
    Task<ApiResponseDto<ProductDto>> UpdateProductAsync(UpdateProductDto dto);
    Task<ApiResponseDto<bool>> DeleteProductAsync(Guid id);
}
