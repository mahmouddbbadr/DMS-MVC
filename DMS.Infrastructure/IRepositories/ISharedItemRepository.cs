using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface ISharedItemRepository: IRepository<SharedItem>
    {
        Task<int> GetSharedWithMeCountAsync(string id);
        Task<int> GetSharedByMeCountAsync(string id);
    }
}
