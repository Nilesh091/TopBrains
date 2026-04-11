using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Repositories;

public class OrderRepository : IOrderRepository
{
  private readonly IApplicationDbContext _dbcontext;

  public OrderRepository(IApplicationDbContext dbcontext)
  {
    _dbcontext = dbcontext;
  }

  public async Task<IEnumerable<Order>> GetAllOrder()
  {
    return await _dbcontext.Orders.ToListAsync();
  }

  public async Task<string> Add(Order order)
  {
    _dbcontext.Orders.Add(order);
    await _dbcontext.SaveChangesAsync();
    return order.Id;
  }

  public async Task<string> Cancel(string id)
  {
    var orderupt = await _dbcontext.Orders.Where(orderdet => orderdet.Id == id).FirstOrDefaultAsync();
    if (orderupt == null)
    {
      return "Order does not exists";
    }

    orderupt.Status = "Cancelled";
    _dbcontext.Orders.Remove(orderupt);
    await _dbcontext.SaveChangesAsync();
    return "Order Cancelled Successfully";
  }

  // public async Task<Order?> GetByCustomerId(string custid)
  // {
  //   var order = await _dbcontext.Orders.Where(orderdet => orderdet.CustomerId == custid).FirstOrDefaultAsync();
  //   return order;
  // }

  public async Task<Order?> GetById(string id)
  {
    var order = await _dbcontext.Orders.Where(orderdet => orderdet.Id == id).FirstOrDefaultAsync();
    return order;
  }
}
