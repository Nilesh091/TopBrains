namespace OrderService.Application.Interfaces.Repository;

/// <summary>
/// Generic repository interface for CRUD operations.
/// </summary>
public interface IRepository<T> where T : class
{
  /// <summary>Gets an entity by ID.</summary>
  Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

  /// <summary>Gets all entities.</summary>
  Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

  /// <summary>Adds a new entity.</summary>
  Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

  /// <summary>Updates an existing entity.</summary>
  Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

  /// <summary>Deletes an entity.</summary>
  Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

  /// <summary>Deletes an entity by instance.</summary>
  Task<bool> DeleteByInstanceAsync(T entity, CancellationToken cancellationToken = default);

  /// <summary>Checks if an entity exists.</summary>
  Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
