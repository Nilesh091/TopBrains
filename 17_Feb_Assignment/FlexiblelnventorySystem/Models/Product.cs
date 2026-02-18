using System;

namespace FlexibleInventorySystem.Models
{
  /// <summary>
  /// Abstract base class for all products
  /// </summary>
  public abstract class Product
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Implement abstract method to get product-specific details
    /// </summary>
    public abstract string GetProductDetails();

    /// <summary>
    /// Virtual method to calculate inventory value. Default: Price * Quantity
    /// </summary>
    public virtual decimal CalculateValue()
    {
      return Price * Quantity;
    }

    /// <summary>
    /// Override ToString() to return product summary
    /// </summary>
    public override string ToString()
    {
      return $"{Id} - {Name} - {Price:C} - Qty: {Quantity}";
    }
  }
}
