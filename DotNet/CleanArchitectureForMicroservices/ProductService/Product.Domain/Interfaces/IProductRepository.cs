using Product.Domain.Entities;

namespace Product.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<ProductEntity>> GetAllAsync();
    Task<ProductEntity> GetByIdAsync(Guid id);
    Task<ProductEntity> CreateAsync(ProductEntity product);
    Task<ProductEntity> UpdateAsync(ProductEntity product);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ProductExistsAsync(Guid id);
}
