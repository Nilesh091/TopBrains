using System;

namespace ECommerce.Domain.Entities
{
    public class Address
    {
        public string Street { get; private set; }
        public string City { get; private set; }

        private Address() { }

        public Address(string street, string city)
        {
            Street = street;
            City = city;
        }
    }
}
