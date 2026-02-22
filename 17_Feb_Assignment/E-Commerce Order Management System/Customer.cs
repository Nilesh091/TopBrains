using System;

namespace E_Commerce_Order_Management_System
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsBlacklisted { get; set; }
    }
}
