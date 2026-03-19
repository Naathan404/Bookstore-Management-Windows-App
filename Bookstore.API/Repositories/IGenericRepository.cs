using System.Linq.Expressions;

namespace Bookstore.API.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIDAsync(int id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
        Task<bool> SaveChangesAsync();
    }
}
