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
        
        public SortSearch(DMSContext context) : base(context)
        {
        }

        public List<TEntity> GetByName(string searchName)
        {
            searchName = searchName?.Trim() ?? "";
            return _dbset
                .Where(f => EF.Functions.Like(f.Name, searchName))
                .ToList();
        }

        public IQueryable<TEntity> SearchAsQueryable(string searchName)
        {
            searchName = searchName?.Trim() ?? "";
            return _dbset
                .Where(f => EF.Functions.Like(f.Name, $"{searchName}%"));
        }
        public IQueryable<TEntity> SortedByNameAsc()
        {
            return _dbset.OrderBy(f => f.Name);
        }
        public IQueryable<TEntity> SortedByNameDesc()
        {
            return _dbset.OrderByDescending(f => f.Name);
        }
        public IQueryable<TEntity> SortedByDate()
        {
            return _dbset.OrderBy(f => f.AddedAt);
        }
        public IQueryable<TEntity> SortedByDateDesc()
        {
            return _dbset.OrderByDescending(f => f.AddedAt);
        }
        public IQueryable<TEntity> GetDeleted()
        {
            return _dbset.Where(e => e.IsDeleted).IgnoreQueryFilters();
        }
    }
}
