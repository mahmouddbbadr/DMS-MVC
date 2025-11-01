using AutoMapper;
using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.Account;
using Microsoft.AspNetCore.Identity;

namespace DMS.Service.Service
{
    public class UserService(UnitOfWork unit, UserManager<AppUser> userManager, IMapper mapper): IUserService
    {

        public async Task<(List<UserOutputViewModel> users, int totalCount, int totalPages)> GetAllBlockedAsnyc(int page, int pageSize)
        {
            var users = await unit.AppUserRepository.GetBlockedUsersAsync();
            var TotalCount = users.Count;
            var totalPages = (int)Math.Ceiling((double)TotalCount / pageSize);
            users = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var mappedUsers = mapper.Map<List<UserOutputViewModel>>(users);
            return (mappedUsers, TotalCount, totalPages);
        }

        public async Task<(List<UserOutputViewModel> users, int totalCount, int totalPages)> GetAllUnBlockedAsnyc(string userId, int page, int pageSize)
        {
            var users = await unit.AppUserRepository.GetUnBlockedUsersAsync(userId);
            var totalCount = users.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            users = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var mappedUsers = mapper.Map<List<UserOutputViewModel>>(users);
            return (mappedUsers, totalCount, totalPages);
        }


        public async Task<(List<UserOutputViewModel> users, int totalCount, int totalPages)> SearchBlockedUsersAsnyc(string email, int page, int pageSize)
        {
            var users = await unit.AppUserRepository.SearchBlockedUsersAsync(email);
            var TotalCount = users.Count;
            var totalPages = (int)Math.Ceiling((double)TotalCount / pageSize);
            users = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var mappedUsers = mapper.Map<List<UserOutputViewModel>>(users);
            return (mappedUsers, TotalCount, totalPages);
        }

        public async Task<(List<UserOutputViewModel> users, int totalCount, int totalPages)> SearchUnBlockedUsersAsnyc(string userId, string email, int page, int pageSize)
        {
            var users = await unit.AppUserRepository.SearchUnBlockedUsersAsync(userId, email);
            var TotalCount = users.Count;
            var totalPages = (int)Math.Ceiling((double)TotalCount / pageSize);
            users = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var mappedUsers = mapper.Map<List<UserOutputViewModel>>(users);
            return (mappedUsers, TotalCount, totalPages);
        }
       
        
       


        public async Task<bool> BlockUserAsnyc(string email)
        {
            var user = await unit.AppUserRepository.GetUserByEmailAsync(email);
            user.IsLocked = true;
            var result = await userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return true;
            }
            return false;
        }





        public async Task<bool> UnBlockUserAsnyc(string email)
        {
            var user = await unit.AppUserRepository.GetUserByEmailAsync(email);
            user.IsLocked = false;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

    }
}
