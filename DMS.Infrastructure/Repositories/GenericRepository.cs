using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DMSContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        public GenericRepository(DMSContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        public IQueryable<TEntity> GetAllAsQueryable()
        {
            return _dbSet.AsNoTracking();
        }
        public async Task<List<TEntity>> GetAllWithPaginationAsync(IQueryable<TEntity> query, int pageNum = 1, int pageSize = 5)
        {
            return await query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
