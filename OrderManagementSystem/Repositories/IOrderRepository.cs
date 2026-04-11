using OrderManagementSystem.Models;

namespace OrderManagementSystem.Repositories;

public interface IOrderRepository
{
  Task<IEnumerable<Order>> GetAllOrder();
  Task<string> Add(Order order);
  Task<Order?> GetById(string id);
  Task<string> Cancel(string id);
  // Task<Order?> GetByCustomerId(string custid);
}
