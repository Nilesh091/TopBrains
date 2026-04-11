using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Repositories
{
  public class OrderRepository : IOrderRepository
  {
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrders() => await _context.Orders.ToListAsync();

    public async Task<Order?> GetOrderById(int id) => await _context.Orders.FindAsync(id);

    public async Task AddOrder(Order order)
    {
      await _context.Orders.AddAsync(order);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateOrder(Order order)
    {
      _context.Orders.Update(order);
      await _context.SaveChangesAsync();
    }

    public async Task<Order> CreateOrder(Order order)
    {
      _context.Orders.Add(order);
      await _context.SaveChangesAsync();
      return order;
    }

    public async Task DeleteOrder(int? id)
    {
      var order = await _context.Orders.FindAsync(id);
      if (order != null)
      {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
      }
    }
  }
}
