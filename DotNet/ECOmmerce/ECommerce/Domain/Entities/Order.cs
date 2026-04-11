using System;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public int CustomerId { get; private set; }

        public List<OrderItem> Items { get; private set; } = new();

        private Order() { }

        public Order(int customerId)
        {
            CustomerId = customerId;
        }

        public void AddItem(Product product, int qty)
        {
            if (product.StockQuantity < qty)
                throw new Exception("Stock not available");

            Items.Add(new OrderItem(product.Id, product.Price, qty));
            product.ReduceStock(qty);
        }
    }
}
