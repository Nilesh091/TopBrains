using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleInventorySystem.Interfaces;
using FlexibleInventorySystem.Models;
using FlexibleInventorySystem.Utilities;

namespace FlexibleInventorySystem.Services
{
  /// <summary>
  /// Main inventory manager implementing IInventoryOperations and IReportGenerator
  /// </summary>
  public class InventoryManager : IInventoryOperations, IReportGenerator
  {
    private readonly List<Product> _products = new List<Product>();
    private readonly object _lockObj = new object();

    public InventoryManager()
    {
    }

    public bool AddProduct(Product product)
    {
      if (product == null)
        return false;
      if (!ProductValidator.ValidateProduct(product, out string errorMessage))
        return false;
      lock (_lockObj)
      {
        if (FindProduct(product.Id) != null)
          return false;
        _products.Add(product);
      }
      return true;
    }

    public bool RemoveProduct(string productId)
    {
      if (string.IsNullOrEmpty(productId))
        return false;
      lock (_lockObj)
      {
        Product p = FindProduct(productId);
        if (p == null)
          return false;
        return _products.Remove(p);
      }
    }

    public Product FindProduct(string productId)
    {
      if (string.IsNullOrEmpty(productId))
        return null;
      foreach (Product p in _products)
      {
        if (string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase))
          return p;
      }
      return null;
    }

    public List<Product> GetProductsByCategory(string category)
    {
      if (string.IsNullOrEmpty(category))
        return new List<Product>();
      return _products.Where(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public bool UpdateQuantity(string productId, int newQuantity)
    {
      if (newQuantity < 0)
        return false;
      Product p = FindProduct(productId);
      if (p == null)
        return false;
      p.Quantity = newQuantity;
      return true;
    }

    public decimal GetTotalInventoryValue()
    {
      decimal total = 0;
      foreach (Product p in _products)
      {
        total += p.CalculateValue();
      }
      return total;
    }

    public List<Product> GetLowStockProducts(int threshold)
    {
      return _products.Where(p => p.Quantity < threshold).ToList();
    }

    public string GenerateInventoryReport()
    {
      var sb = new System.Text.StringBuilder();
      sb.AppendLine("================================");
      sb.AppendLine("INVENTORY REPORT");
      sb.AppendLine("================================");
      sb.AppendLine($"Total Products: {_products.Count}");
      sb.AppendLine($"Total Value: {GetTotalInventoryValue():C}");
      sb.AppendLine();
      sb.AppendLine("Product List:");
      foreach (Product p in _products)
      {
        sb.AppendLine($"{p.Id} - {p.Name} - {p.Category} - {p.Quantity} - {p.CalculateValue():C}");
      }
      return sb.ToString();
    }

    public string GenerateCategorySummary()
    {
      var sb = new System.Text.StringBuilder();
      sb.AppendLine("CATEGORY SUMMARY");
      var groups = _products.GroupBy(p => p.Category ?? "");
      foreach (var group in groups)
      {
        decimal value = 0;
        foreach (Product p in group)
          value += p.CalculateValue();
        sb.AppendLine($"{group.Key}: {group.Count()} items - Total Value: {value:C}");
      }
      return sb.ToString();
    }

    public string GenerateValueReport()
    {
      if (_products.Count == 0)
        return "No products in inventory.";
      var sb = new System.Text.StringBuilder();
      Product mostValuable = _products.OrderByDescending(p => p.CalculateValue()).First();
      Product leastValuable = _products.OrderBy(p => p.CalculateValue()).First();
      decimal avgPrice = _products.Average(p => p.Price);
      var sortedPrices = _products.Select(p => p.Price).OrderBy(x => x).ToList();
      int n = sortedPrices.Count;
      decimal medianPrice = n % 2 == 1
        ? sortedPrices[n / 2]
        : (sortedPrices[n / 2 - 1] + sortedPrices[n / 2]) / 2;
      var aboveAverage = _products.Where(p => p.Price > avgPrice).ToList();
      sb.AppendLine("VALUE REPORT");
      sb.AppendLine($"Most valuable product: {mostValuable.Id} - {mostValuable.Name} - {mostValuable.CalculateValue():C}");
      sb.AppendLine($"Least valuable product: {leastValuable.Id} - {leastValuable.Name} - {leastValuable.CalculateValue():C}");
      sb.AppendLine($"Average price: {avgPrice:C}");
      sb.AppendLine($"Median price: {medianPrice:C}");
      sb.AppendLine($"Products above average price: {aboveAverage.Count}");
      foreach (Product p in aboveAverage)
      {
        sb.AppendLine($"  {p.Id} - {p.Name} - {p.Price:C}");
      }
      return sb.ToString();
    }

    public string GenerateExpiryReport(int daysThreshold)
    {
      var sb = new System.Text.StringBuilder();
      sb.AppendLine($"EXPIRY REPORT (within {daysThreshold} days)");
      foreach (Product p in _products)
      {
        if (p is GroceryProduct gp)
        {
          int days = gp.DaysUntilExpiry();
          if (days >= 0 && days <= daysThreshold)
          {
            sb.AppendLine($"{gp.Id} - {gp.Name} - Expires in {days} days - {gp.ExpiryDate:yyyy-MM-dd}");
          }
        }
      }
      return sb.ToString();
    }

    public IEnumerable<Product> SearchProducts(Func<Product, bool> predicate)
    {
      if (predicate == null)
        return new List<Product>();
      return _products.Where(predicate);
    }

    public void ApplyCategoryDiscount(string category, decimal discountPercentage)
    {
      if (discountPercentage <= 0 || discountPercentage > 100)
        return;
      decimal factor = (100 - discountPercentage) / 100m;
      foreach (Product p in _products)
      {
        if (string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase))
        {
          p.Price = p.Price * factor;
        }
      }
    }

    public int GetTotalProductCount()
    {
      return _products.Count;
    }

    public IEnumerable<string> GetCategories()
    {
      return _products.Select(p => p.Category ?? "").Distinct();
    }
  }
}
