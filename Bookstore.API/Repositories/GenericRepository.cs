using Bookstore.API.Data;
using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.API.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIDAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity item)
        {
            await _dbSet.AddAsync(item);
        }
        public void Update(TEntity item)
        {
            _dbSet.Update(item);
        }

        public void Delete(TEntity item)
        {
            _dbSet.Remove(item);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbSet.FirstOrDefaultAsync(predicate);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
