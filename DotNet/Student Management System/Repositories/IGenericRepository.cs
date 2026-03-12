using System;
using System.Linq.Expressions;
namespace Student_Management_System.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(object id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    }
}
