using FlexibleInventorySystem.Models;

namespace FlexibleInventorySystem.Utilities
{
  /// <summary>
  /// Validation helper class for products
  /// </summary>
  public static class ProductValidator
  {
    /// <summary>
    /// Validate product data. Check ID, Name, Price > 0, Quantity >= 0
    /// </summary>
    public static bool ValidateProduct(Product product, out string? errorMessage)
    {
      errorMessage = null;
      if (product == null)
      {
        errorMessage = "Product cannot be null.";
        return false;
      }
      if (string.IsNullOrWhiteSpace(product.Id))
      {
        errorMessage = "Product ID cannot be null or empty.";
        return false;
      }
      if (string.IsNullOrWhiteSpace(product.Name))
      {
        errorMessage = "Product Name cannot be null or empty.";
        return false;
      }
      if (product.Price <= 0)
      {
        errorMessage = "Price must be greater than 0.";
        return false;
      }
      if (product.Quantity < 0)
      {
        errorMessage = "Quantity cannot be negative.";
        return false;
      }
      return true;
    }

    /// <summary>
    /// Validate electronic product specific rules
    /// </summary>
    public static bool ValidateElectronicProduct(ElectronicProduct product, out string? errorMessage)
    {
      errorMessage = null;
      if (product == null)
      {
        errorMessage = "Product cannot be null.";
        return false;
      }
      if (!ValidateProduct(product, out errorMessage))
        return false;
      if (string.IsNullOrWhiteSpace(product.Brand))
      {
        errorMessage = "Brand cannot be null or empty.";
        return false;
      }
      if (product.WarrantyMonths < 0)
      {
        errorMessage = "Warranty months cannot be negative.";
        return false;
      }
      return true;
    }

    /// <summary>
    /// Validate grocery product specific rules
    /// </summary>
    public static bool ValidateGroceryProduct(GroceryProduct product, out string? errorMessage)
    {
      errorMessage = null;
      if (product == null)
      {
        errorMessage = "Product cannot be null.";
        return false;
      }
      if (!ValidateProduct(product, out errorMessage))
        return false;
      if (product.Weight < 0)
      {
        errorMessage = "Weight cannot be negative.";
        return false;
      }
      return true;
    }

    /// <summary>
    /// Validate clothing product specific rules
    /// </summary>
    public static bool ValidateClothingProduct(ClothingProduct product, out string? errorMessage)
    {
      errorMessage = null;
      if (product == null)
      {
        errorMessage = "Product cannot be null.";
        return false;
      }
      if (!ValidateProduct(product, out errorMessage))
        return false;
      if (string.IsNullOrWhiteSpace(product.Size))
      {
        errorMessage = "Size cannot be null or empty.";
        return false;
      }
      if (!product.IsValidSize())
      {
        errorMessage = "Invalid size. Valid sizes: XS, S, M, L, XL, XXL.";
        return false;
      }
      return true;
    }
  }
}
