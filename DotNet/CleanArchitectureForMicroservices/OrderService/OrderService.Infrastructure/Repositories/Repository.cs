using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces.Repository;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation with basic CRUD operations.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
  protected readonly OrderServiceDbContext Context;
  protected readonly DbSet<T> DbSet;

  public Repository(OrderServiceDbContext context)
  {
    Context = context;
    DbSet = context.Set<T>();
  }

  /// <inheritdoc />
  public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
  {
    return await DbSet.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
  }

  /// <inheritdoc />
  public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return await DbSet.ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    await DbSet.AddAsync(entity, cancellationToken);
    return entity;
  }

  /// <inheritdoc />
  public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    DbSet.Update(entity);
    await Task.CompletedTask;
    return entity;
  }

  /// <inheritdoc />
  public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
  {
    var entity = await GetByIdAsync(id, cancellationToken);
    if (entity == null)
      return false;

    DbSet.Remove(entity);
    return true;
  }

  /// <inheritdoc />
  public virtual async Task<bool> DeleteByInstanceAsync(T entity, CancellationToken cancellationToken = default)
  {
    DbSet.Remove(entity);
    await Task.CompletedTask;
    return true;
  }

  /// <inheritdoc />
  public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
  {
    var entity = await GetByIdAsync(id, cancellationToken);
    return entity != null;
  }
}
