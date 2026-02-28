using System;
using OrderManagement.Model;

namespace OrderManagement.Dto
{
    public class CartDto
    {
        public int ConsumerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
