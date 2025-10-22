using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;

namespace DMS.Infrastructure.IRepositories
{
    public interface ISharedItemRepository: IRepository<SharedItem>
    {
        public Task<List<SharedItem>> GetSharedByMeQueryAsync(string userId,string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 5);
    }
}
