using System;

namespace FlexibleInventorySystem.Models
{
  /// <summary>
  /// Clothing product class
  /// </summary>
  public class ClothingProduct : Product
  {
    public string Size { get; set; }
    public string Color { get; set; }
    public string Material { get; set; }
    public string Gender { get; set; }
    public string Season { get; set; }

    private static readonly string[] ValidSizes = { "XS", "S", "M", "L", "XL", "XXL" };

    /// <summary>
    /// Override GetProductDetails for clothing items
    /// </summary>
    public override string GetProductDetails()
    {
      return $"Size: {Size}, Color: {Color}, Material: {Material}";
    }

    /// <summary>
    /// Check if size is valid. Valid sizes: XS, S, M, L, XL, XXL
    /// </summary>
    public bool IsValidSize()
    {
      if (string.IsNullOrEmpty(Size)) return false;
      for (int i = 0; i < ValidSizes.Length; i++)
      {
        if (string.Equals(Size, ValidSizes[i], StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Override CalculateValue to apply 15% discount for off-season items
    /// </summary>
    public override decimal CalculateValue()
    {
      decimal baseValue = Price * Quantity;
      string currentSeason = GetCurrentSeason();
      if (!string.IsNullOrEmpty(Season) && !string.Equals(Season, currentSeason, StringComparison.OrdinalIgnoreCase) && !string.Equals(Season, "All-season", StringComparison.OrdinalIgnoreCase))
      {
        return baseValue * 0.85m;
      }
      return baseValue;
    }

    private static string GetCurrentSeason()
    {
      int month = DateTime.Now.Month;
      if (month >= 6 && month <= 8) return "Summer";
      if (month == 12 || month <= 2) return "Winter";
      return "All-season";
    }
  }
}
