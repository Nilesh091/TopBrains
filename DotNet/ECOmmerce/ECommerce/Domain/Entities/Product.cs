using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; private set; }

        public int StockQuantity { get; private set; }

        private Product() { }

        public Product(string name, decimal price, int stock)
        {
            Name = name;
            Price = price;
            StockQuantity = stock;
        }

        public void ReduceStock(int qty)
        {
            if (qty > StockQuantity)
                throw new Exception("Insufficient stock");

            StockQuantity -= qty;
        }
    }
}
