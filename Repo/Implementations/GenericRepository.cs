using ChatApp.Models;
using ChatApp.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repo.Implementations
{

        public class GenericRepository<T> : IGenericRepository<T> where T : class
        {
            protected readonly AppDbContext _context;
            protected readonly DbSet<T> _dbSet;

            public GenericRepository(AppDbContext context)
            {
                _context = context;
                _dbSet = context.Set<T>();
            }

            public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

            public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

            public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

            public void Update(T entity) => _dbSet.Update(entity);

            public void Delete(T entity) => _dbSet.Remove(entity);

            public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        }

    }

