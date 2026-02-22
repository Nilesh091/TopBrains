using System;

namespace E_Commerce_Order_Management_System
{
    public class FlatDiscount : IDiscountStrategy
    {
        private readonly decimal _amount;

        public FlatDiscount(decimal amount)
        {
            _amount = amount;
        }

        public decimal ApplyDiscount(decimal totalAmount)
        {
            return totalAmount - _amount;
        }
    }
}
