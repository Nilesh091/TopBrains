using System;

namespace E_Commerce_Order_Management_System
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public void ReduceStock(int quantity)
        {
            if (Stock < quantity)
                throw new OutOfStockException($"Product {Name} is out of stock.");

            Stock -= quantity;
        }
    }
}
