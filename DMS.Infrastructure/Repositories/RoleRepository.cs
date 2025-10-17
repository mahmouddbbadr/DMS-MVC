using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Repository
{
    public class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(DMSContext context):base(context)
        {
            
        }

        public async Task<List<IdentityRole>> GetByNameAsync(string name)
        {
            return await _context.Roles.Where(u => u.Name == name).ToListAsync();
        }

    }
}
