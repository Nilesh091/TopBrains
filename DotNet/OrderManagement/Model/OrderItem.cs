using System;
using System.Net.ServerSentEvents;

namespace OrderManagement.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
