using System;

namespace E_Commerce_Order_Management_System
{
    public class Order
    {
        public int OrderId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }

        public decimal GetTotalAmount()
        {
            return Items.Sum(i => i.TotalPrice());
        }

        public void AddItem(Product product, int quantity)
        {
            product.ReduceStock(quantity);

            Items.Add(new OrderItem
            {
                Product = product,
                Quantity = quantity
            });
        }

        public void CancelOrder()
        {
            if (Status == OrderStatus.Shipped)
                throw new OrderAlreadyShippedException("Cannot cancel shipped order.");

            Status = OrderStatus.Cancelled;
        }
    }
}
