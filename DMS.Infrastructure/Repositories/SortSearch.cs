using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;

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
        public IQueryable<TEntity> TrashedSearchAsQueryable(string searchName, string userId)
        {
            searchName = searchName?.Trim() ?? "";
            return GetDeleted(userId)
                .Where(f => EF.Functions.Like(f.Name, $"%{searchName}%"));
        }
        public IQueryable<TEntity> StarredSearchAsQueryable(string searchName, string userId)
        {
            searchName = searchName?.Trim() ?? "";
            return GetStarred(userId)
                .Where(f => EF.Functions.Like(f.Name, $"%{searchName}%"));
        }

        public IQueryable<TEntity> SortedByNameAsc() => _dbSet.OrderBy(f => f.Name);
        public IQueryable<TEntity> SortedByNameDesc() => _dbSet.OrderByDescending(f => f.Name);
        public IQueryable<TEntity> SortedByDate() => _dbSet.OrderBy(f => f.AddedAt);
        public IQueryable<TEntity> SortedByDateDesc() => _dbSet.OrderByDescending(f => f.AddedAt);
        public IQueryable<TEntity> GetDeleted(string userId) => _dbSet.Where(e => e.IsDeleted && e.OwnerId == userId).IgnoreQueryFilters();
        public async Task<TEntity?> GetDeletedById(string id, string userId) =>
            await GetDeleted(userId).SingleOrDefaultAsync(e => e.Id == id);
        public IQueryable<TEntity> GetStarred(string userId) => _dbSet.Where(e => e.IsStarred && e.OwnerId == userId);


        // for security
        public async Task<TEntity?> GetByOwnerAsync(string entityId, string ownerId)
        {
            return await _dbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == entityId && e.OwnerId == ownerId);
        }
    }
}
