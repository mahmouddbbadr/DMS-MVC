using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;
using System.Linq.Expressions;

namespace DMS.Infrastructure.IRepositories
{
    public interface ISharedItemRepository: IRepository<SharedItem>
    {
        //public Task<List<SharedItem>> GetSharedByMeQueryAsync(string userId,string search = "",
        //    string sortOrder = "dateDesc",
        //    int page = 1,
        //    int pageSize = 5);
        public Task<bool> AnyAsync(Expression<Func<SharedItem, bool>> predicate);
        public Task<List<SharedItem>> GetSharedItemsByUserAndItemAsync(string itemId, string itemType, string userId);
        Task<SharedItem> FirstOrDefaultAsync(Expression<Func<SharedItem, bool>> predicate);

        Task<int> GetSharedWithMeCountAsync(string id);
        Task<int> GetSharedByMeCountAsync(string id);

        /////////////////
        IQueryable<SharedItem> GetSharedWithMeQuery(
            string userId,
            string search,
            string sortOrder,
            int page,
            int pageSize);
        IQueryable<SharedItem> GetSharedByMeQuery(
            string userId,
            string search,
            string sortOrder,
            int page,
            int pageSize);
        Task<SharedItem?> GetSharedFolderByIdAuthorizeAsync(string folderId, string userId);
    }
}
