using System;

namespace OrderManagement.Model
{
    public class Cart
    {
        public int Id { get; set; }
        public int ConsumerId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
