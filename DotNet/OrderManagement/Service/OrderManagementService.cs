using System;
using OrderManagement.Model;
using OrderManagement.Model.Context;

namespace OrderManagement.Service
{
    public class OrderManagementService
    {
        Context context = new Context();
        public void AddCart()
        {

        }
        public void placeOrder()
        {

        }
        public void ViewCart()
        {
            var detailsofOrder = context.Carts.ToList();
            if (detailsofOrder == null)
            {
                Console.WriteLine("No product added");
            }
            foreach (var v in detailsofOrder)
            {
                Console.WriteLine("Consumer Id" + v.ConsumerId);
                foreach (var items in v.OrderItems)
                {
                    Console.WriteLine("Item name:" + items.Item.ItemName + " Quantity: " + items.Quantity);
                }
            }
        }


    }
}
