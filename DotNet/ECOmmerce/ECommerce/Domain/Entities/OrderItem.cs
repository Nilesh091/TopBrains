using System;

namespace ECommerce.Domain.Entities
{
    public class OrderItem
    {
        public int ProductId { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        private OrderItem() { }

        public OrderItem(int productId, decimal price, int qty)
        {
            ProductId = productId;
            UnitPrice = price;
            Quantity = qty;
        }

    }
}
