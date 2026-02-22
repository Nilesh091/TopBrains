using System;

namespace E_Commerce_Order_Management_System
{
    public class OutOfStockException : Exception
    {
        public OutOfStockException(string message) : base(message) { }
    }

    public class OrderAlreadyShippedException : Exception
    {
        public OrderAlreadyShippedException(string message) : base(message) { }
    }

    public class CustomerBlacklistedException : Exception
    {
        public CustomerBlacklistedException(string message) : base(message) { }
    }
}
