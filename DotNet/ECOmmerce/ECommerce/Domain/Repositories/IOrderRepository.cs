using System;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
    }
}
