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
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(DMSContext context):base(context)
        {
            
        }
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u=> u.Email.ToLower() == email.ToLower());
        }
        public async Task<AppUser> GetBlockedUserByEmailAsync(string email)
        {
            return await _context.AppUsers.Where(u => u.IsLocked).FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
        public async Task<int> GetBlockUsers()
        {
            return await _context.AppUsers.Where(u => u.IsLocked).CountAsync();
        }
        public  int GetFoldersCountAsync(string id)
        {
            return  _context.Folders.Where(d => d.Owner.Id == id).Count();
        }
        public  int GetDocumentsCountAsync(string id)
        {
            return  _context.Documents.Where(d=> d.Folder.Owner.Id == id).Count();
        }

        public async Task<List<AppUser>> GetBlockedUsersAsync()
        {
            return await _context.AppUsers.Where(u => u.IsLocked).ToListAsync();
        }


        public async Task<List<AppUser>> GetUnBlockedUsersAsync()
        {
            return await _context.AppUsers.Where(u => !u.IsLocked).ToListAsync();

        }

        public async Task<List<AppUser>> SearchBlockedUsersAsync(string email)
        {
            return await _context.AppUsers.Where(u => u.IsLocked && u.Email.ToLower().StartsWith(email.ToLower())).ToListAsync();
        }

        public async Task<List<AppUser>> SearchUnBlockedUsersAsync(string email)
        {
            return await _context.AppUsers.Where(u => !u.IsLocked && u.Email.ToLower().StartsWith(email.ToLower())).ToListAsync();

        }
    }
}
