using System;
using FlexibleInventorySystem.Services;
using FlexibleInventorySystem.Models;

namespace FlexibleInventorySystem
{
  /// <summary>
  /// Console user interface for the inventory system
  /// </summary>
  class Program
  {
    private static InventoryManager _inventory = new InventoryManager();

    static void Main(string[] args)
    {
      while (true)
      {
        DisplayMenu();
        string choice = Console.ReadLine()?.Trim();

        switch (choice)
        {
          case "1":
            AddProductMenu();
            break;
          case "2":
            RemoveProductMenu();
            break;
          case "3":
            UpdateQuantityMenu();
            break;
          case "4":
            FindProductMenu();
            break;
          case "5":
            ViewAllProductsMenu();
            break;
          case "6":
            GenerateReportsMenu();
            break;
          case "7":
            CheckLowStockMenu();
            break;
          case "8":
            Console.WriteLine("Goodbye.");
            return;
          default:
            Console.WriteLine("Invalid option. Try again.");
            break;
        }
        Console.WriteLine();
      }
    }

    static void DisplayMenu()
    {
      Console.WriteLine("================================");
      Console.WriteLine("  FLEXIBLE INVENTORY SYSTEM");
      Console.WriteLine("================================");
      Console.WriteLine("1. Add Product");
      Console.WriteLine("2. Remove Product");
      Console.WriteLine("3. Update Quantity");
      Console.WriteLine("4. Find Product");
      Console.WriteLine("5. View All Products");
      Console.WriteLine("6. Generate Reports");
      Console.WriteLine("7. Check Low Stock");
      Console.WriteLine("8. Exit");
      Console.WriteLine("================================");
      Console.Write("Enter your choice (1-8): ");
    }

    static void AddProductMenu()
    {
      Console.WriteLine("Select product type: 1=Electronic, 2=Grocery, 3=Clothing");
      string typeInput = Console.ReadLine()?.Trim();
      if (typeInput != "1" && typeInput != "2" && typeInput != "3")
      {
        Console.WriteLine("Invalid type.");
        return;
      }

      Console.Write("Product ID: ");
      string id = Console.ReadLine()?.Trim();
      Console.Write("Name: ");
      string name = Console.ReadLine()?.Trim();
      Console.Write("Price: ");
      if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
      {
        Console.WriteLine("Invalid price.");
        return;
      }
      Console.Write("Quantity: ");
      if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
      {
        Console.WriteLine("Invalid quantity.");
        return;
      }
      Console.Write("Category: ");
      string category = Console.ReadLine()?.Trim() ?? "";

      Product product = null;
      if (typeInput == "1")
      {
        var p = new ElectronicProduct
        {
          Id = id,
          Name = name,
          Price = price,
          Quantity = quantity,
          Category = category,
          DateAdded = DateTime.Now
        };
        Console.Write("Brand: ");
        p.Brand = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Warranty (months): ");
        int.TryParse(Console.ReadLine(), out int warranty);
        p.WarrantyMonths = warranty;
        Console.Write("Voltage: ");
        p.Voltage = Console.ReadLine()?.Trim() ?? "";
        product = p;
      }
      else if (typeInput == "2")
      {
        var p = new GroceryProduct
        {
          Id = id,
          Name = name,
          Price = price,
          Quantity = quantity,
          Category = category,
          DateAdded = DateTime.Now
        };
        Console.Write("Expiry date (yyyy-mm-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime expiry))
          p.ExpiryDate = expiry;
        else
          p.ExpiryDate = DateTime.Now.AddDays(7);
        Console.Write("Is perishable (y/n): ");
        p.IsPerishable = string.Equals(Console.ReadLine()?.Trim(), "y", StringComparison.OrdinalIgnoreCase);
        Console.Write("Weight: ");
        double.TryParse(Console.ReadLine(), out double weight);
        p.Weight = weight;
        Console.Write("Storage temperature: ");
        p.StorageTemperature = Console.ReadLine()?.Trim() ?? "";
        product = p;
      }
      else if (typeInput == "3")
      {
        var p = new ClothingProduct
        {
          Id = id,
          Name = name,
          Price = price,
          Quantity = quantity,
          Category = category,
          DateAdded = DateTime.Now
        };
        Console.Write("Size (XS/S/M/L/XL/XXL): ");
        p.Size = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Color: ");
        p.Color = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Material: ");
        p.Material = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Gender (Men/Women/Unisex): ");
        p.Gender = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Season (Summer/Winter/All-season): ");
        p.Season = Console.ReadLine()?.Trim() ?? "";
        product = p;
      }

      if (product != null)
      {
        if (_inventory.AddProduct(product))
          Console.WriteLine("Product added successfully.");
        else
          Console.WriteLine("Failed to add product. Check ID is unique and data is valid.");
      }
    }

    static void RemoveProductMenu()
    {
      Console.Write("Enter product ID to remove: ");
      string id = Console.ReadLine()?.Trim();
      if (string.IsNullOrEmpty(id))
      {
        Console.WriteLine("ID cannot be empty.");
        return;
      }
      if (_inventory.RemoveProduct(id))
        Console.WriteLine("Product removed.");
      else
        Console.WriteLine("Product not found.");
    }

    static void UpdateQuantityMenu()
    {
      Console.Write("Enter product ID: ");
      string id = Console.ReadLine()?.Trim();
      Console.Write("Enter new quantity: ");
      if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 0)
      {
        Console.WriteLine("Invalid quantity.");
        return;
      }
      if (_inventory.UpdateQuantity(id, qty))
        Console.WriteLine("Quantity updated.");
      else
        Console.WriteLine("Product not found or invalid quantity.");
    }

    static void FindProductMenu()
    {
      Console.Write("Enter product ID: ");
      string id = Console.ReadLine()?.Trim();
      Product p = _inventory.FindProduct(id);
      if (p == null)
      {
        Console.WriteLine("Product not found.");
        return;
      }
      Console.WriteLine(p.ToString());
      Console.WriteLine(p.GetProductDetails());
    }

    static void ViewAllProductsMenu()
    {
      Console.WriteLine(_inventory.GenerateInventoryReport());
    }

    static void GenerateReportsMenu()
    {
      Console.WriteLine("1. Inventory Report  2. Category Summary  3. Value Report  4. Expiry Report");
      string sub = Console.ReadLine()?.Trim();
      if (sub == "1")
        Console.WriteLine(_inventory.GenerateInventoryReport());
      else if (sub == "2")
        Console.WriteLine(_inventory.GenerateCategorySummary());
      else if (sub == "3")
        Console.WriteLine(_inventory.GenerateValueReport());
      else if (sub == "4")
      {
        Console.Write("Days threshold: ");
        if (int.TryParse(Console.ReadLine(), out int days))
          Console.WriteLine(_inventory.GenerateExpiryReport(days));
        else
          Console.WriteLine(_inventory.GenerateExpiryReport(7));
      }
      else
        Console.WriteLine("Invalid option.");
    }

    static void CheckLowStockMenu()
    {
      Console.Write("Enter low stock threshold: ");
      if (!int.TryParse(Console.ReadLine(), out int threshold))
        threshold = 10;
      var low = _inventory.GetLowStockProducts(threshold);
      if (low.Count == 0)
        Console.WriteLine("No low stock products.");
      else
      {
        Console.WriteLine($"Products with quantity below {threshold}:");
        foreach (Product p in low)
          Console.WriteLine($"  {p.Id} - {p.Name} - Qty: {p.Quantity}");
      }
    }
  }
}
