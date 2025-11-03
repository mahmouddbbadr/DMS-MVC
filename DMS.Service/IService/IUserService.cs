using DMS.Service.ModelViews.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.IService
{
    public interface IUserService
    {
        Task<(List<UserOutputViewModel> users, int totalCount, int totalPages)> GetAllUnBlockedAsnyc(string userId, int page, int pageSize);
        Task<(List<UserOutputViewModel> users,  int totalCount,int totalPages)> GetAllBlockedAsnyc(int page, int pageSize);
        Task<bool> BlockUserAsnyc(string email);
        Task<bool> UnBlockUserAsnyc(string email);
        Task<(List<UserOutputViewModel> users,  int totalCount,int totalPages)> SearchBlockedUsersAsnyc(string email, int page, int pageSize);
        Task<(List<UserOutputViewModel> users,  int totalCount,int totalPages)> SearchUnBlockedUsersAsnyc(string userId, string email, int page, int pageSize);
    }
}
