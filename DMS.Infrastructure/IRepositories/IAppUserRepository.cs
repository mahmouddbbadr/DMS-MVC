using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface IAppUserRepository: IRepository<AppUser>
    {
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<AppUser> GetBlockedUserByEmailAsync(string email);
        Task<List<AppUser>> GetUnBlockedUsersAsync(string userId);
        Task<List<AppUser>> GetBlockedUsersAsync();
        Task<List<AppUser>> SearchBlockedUsersAsync(string email);
        Task<List<AppUser>> SearchUnBlockedUsersAsync(string userId, string email);
        int GetDocumentsCountAsync(string id);
        int GetFoldersCountAsync(string id);
        Task<int> GetBlockUsers();
    }
}
