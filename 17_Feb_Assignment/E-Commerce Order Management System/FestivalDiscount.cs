using System;

namespace E_Commerce_Order_Management_System
{
    public class FestivalDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal totalAmount)
        {
            return totalAmount - (totalAmount * 0.20m);
        }
    }
}
