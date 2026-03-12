using System;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Context;
using System.Linq.Expressions;
namespace Student_Management_System.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //DependencyInjection
        private readonly AppDbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
            }
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);

        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
