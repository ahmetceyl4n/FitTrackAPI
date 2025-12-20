using FitnessApp.Application.Common.Interfaces;
using FitnessApp.Domain.Common;
using FitnessApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAll(bool trackChanges = false)
        {
            return !trackChanges ? _dbSet.AsNoTracking() : _dbSet;
        }

        public async Task<T> GetByIdAsync(Guid id, bool trackChanges = false)
        {
            return !trackChanges
                ? await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
                : await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            _dbSet.Update(entity);
            
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);

        }
    }
}