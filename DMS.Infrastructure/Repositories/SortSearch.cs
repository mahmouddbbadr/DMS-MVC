using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repository
{
    public class SortSearch<TEntity> : GenericRepository<TEntity>, ISortSearch<TEntity>
    where TEntity : class, IBaseEntity
    {
        public SortSearch(DMSContext context) : base(context) { }

        public async Task<List<TEntity>> GetByNameAsync(string searchName)
        {
            searchName = searchName?.Trim() ?? "";
            return await _dbSet
                .Where(f => EF.Functions.Like(f.Name, $"%{searchName}%"))
                .ToListAsync();
        }

        public IQueryable<TEntity> SearchAsQueryable(string searchName)
        {
            searchName = searchName?.Trim() ?? "";
            return _dbSet
                .Where(f => EF.Functions.Like(f.Name, $"%{searchName}%"));
        }

        public IQueryable<TEntity> SortedByNameAsc() => _dbSet.OrderBy(f => f.Name);
        public IQueryable<TEntity> SortedByNameDesc() => _dbSet.OrderByDescending(f => f.Name);
        public IQueryable<TEntity> SortedByDate() => _dbSet.OrderBy(f => f.AddedAt);
        public IQueryable<TEntity> SortedByDateDesc() => _dbSet.OrderByDescending(f => f.AddedAt);
        public IQueryable<TEntity> GetDeleted() => _dbSet.Where(e => e.IsDeleted).IgnoreQueryFilters();
    }

}
