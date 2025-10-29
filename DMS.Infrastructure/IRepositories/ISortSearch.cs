
namespace DMS.Infrastructure.IRepositories
{
    public interface ISortSearch<TEntity> where TEntity : class
    {
        public Task<List<TEntity>> GetByNameAsync(string searchName);
        public IQueryable<TEntity> SearchAsQueryable(string searchName);
        public IQueryable<TEntity> SortedByNameAsc();
        public IQueryable<TEntity> SortedByNameDesc();
        public IQueryable<TEntity> SortedByDate();
        public IQueryable<TEntity> SortedByDateDesc();
        IQueryable<TEntity> GetDeleted(string userId);
        IQueryable<TEntity> TrashedSearchAsQueryable(string searchName, string userId);
        Task<TEntity?> GetDeletedById(string id, string userId);
        IQueryable<TEntity> GetStarred(string userId);
        IQueryable<TEntity> StarredSearchAsQueryable(string searchName, string userId);

        // for security
        Task<TEntity?> GetByOwnerAsync(string entityId, string ownerId);
    }
}
