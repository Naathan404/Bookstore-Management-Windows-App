using Bookstore.API.Models;
using System.Linq.Expressions;

namespace Bookstore.API.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIDAsync(TKey id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        Task<bool> SaveChangesAsync();
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
