using System;

namespace E_Commerce_Order_Management_System
{
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice()
        {
            return Product.Price * Quantity;
        }
    }
}
