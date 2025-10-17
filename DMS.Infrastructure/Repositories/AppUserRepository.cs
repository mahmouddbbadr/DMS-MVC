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

        public async Task<List<AppUser>> GetByNameAsync(string name)
        {
            return await _context.AppUsers.Where(u => u.FName == name).ToListAsync();
        }

    }
}
