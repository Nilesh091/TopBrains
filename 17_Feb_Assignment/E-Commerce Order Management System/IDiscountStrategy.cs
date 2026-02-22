using System;

namespace E_Commerce_Order_Management_System
{
    public interface IDiscountStrategy
    {
        decimal ApplyDiscount(decimal totalAmount);
    }
}
