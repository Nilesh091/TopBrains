using System;

namespace FlexibleInventorySystem.Models
{
  /// <summary>
  /// Electronic product class
  /// </summary>
  public class ElectronicProduct : Product
  {
    public string Brand { get; set; }
    public int WarrantyMonths { get; set; }
    public string Voltage { get; set; }
    public bool IsRefurbished { get; set; }

    /// <summary>
    /// Override GetProductDetails to include electronic specifics
    /// </summary>
    public override string GetProductDetails()
    {
      return $"Brand: {Brand}, Model: {Name}, Warranty: {WarrantyMonths} months";
    }

    /// <summary>
    /// Calculate warranty expiration date
    /// </summary>
    public DateTime GetWarrantyExpiryDate()
    {
      return DateAdded.AddMonths(WarrantyMonths);
    }

    /// <summary>
    /// Check if warranty is still valid
    /// </summary>
    public bool IsWarrantyValid()
    {
      return DateTime.Now < GetWarrantyExpiryDate();
    }
  }
}
