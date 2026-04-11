using OrderService.Models;

namespace OrderService.Repositories
{
  public interface IOrderRepository
  {
    Task<IEnumerable<Order>> GetAllOrders();
    Task<Order?> GetOrderById(int id);
    Task<Order> CreateOrder(Order order);
    Task AddOrder(Order order);
    Task UpdateOrder(Order order);
    Task DeleteOrder(int? id);
  }
}
