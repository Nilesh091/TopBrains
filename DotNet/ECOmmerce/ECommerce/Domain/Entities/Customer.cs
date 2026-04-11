using System;

namespace ECommerce.Domain.Entities
{
    public class Customer
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string Email { get; private set; }

        public Address Address { get; private set; }

        private Customer() { }

        public Customer(string name, string email, Address address)
        {
            FirstName = name;
            Email = email;
            Address = address;
        }
    }
}
