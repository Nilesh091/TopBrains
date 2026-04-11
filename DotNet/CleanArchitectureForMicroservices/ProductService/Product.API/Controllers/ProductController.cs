using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTOs;
using Product.Application.Interfaces;

namespace Product.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all active products
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _service.GetAllProductsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await _service.GetProductByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Create a new product (Admin only)
    /// </summary>
    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _service.CreateProductAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result);
    }

    /// <summary>
    /// Update an existing product (Admin only)
    /// </summary>
    [Authorize(Roles = "Seller")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (id != dto.Id)
            return BadRequest(new ApiResponseDto<ProductDto>
            {
                Success = false,
                Message = "ID mismatch"
            });

        var result = await _service.UpdateProductAsync(dto);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete a product (Admin only)
    /// </summary>
    [Authorize(Roles = "Seller")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await _service.DeleteProductAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
