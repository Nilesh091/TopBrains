using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Data;

public interface IApplicationDbContext
{
  DbSet<Order> Orders { get; set; }
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
