using System;

namespace FlexibleInventorySystem.Models
{
  /// <summary>
  /// Grocery product class
  /// </summary>
  public class GroceryProduct : Product
  {
    public DateTime ExpiryDate { get; set; }
    public bool IsPerishable { get; set; }
    public double Weight { get; set; }
    public string StorageTemperature { get; set; }

    /// <summary>
    /// Override GetProductDetails for grocery items. Include expiry information.
    /// </summary>
    public override string GetProductDetails()
    {
      return $"Expiry: {ExpiryDate:yyyy-MM-dd}, Perishable: {IsPerishable}, Weight: {Weight} kg, Storage: {StorageTemperature}";
    }

    /// <summary>
    /// Check if product is expired
    /// </summary>
    public bool IsExpired()
    {
      return DateTime.Now > ExpiryDate;
    }

    /// <summary>
    /// Calculate days until expiry. Return negative if expired.
    /// </summary>
    public int DaysUntilExpiry()
    {
      TimeSpan diff = ExpiryDate.Date - DateTime.Now.Date;
      return (int)diff.TotalDays;
    }

    /// <summary>
    /// Override CalculateValue to apply 20% discount if within 3 days of expiry
    /// </summary>
    public override decimal CalculateValue()
    {
      decimal baseValue = Price * Quantity;
      int days = DaysUntilExpiry();
      if (days >= 0 && days <= 3)
      {
        return baseValue * 0.80m;
      }
      return baseValue;
    }
  }
}
