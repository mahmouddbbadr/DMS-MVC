using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(DMSContext context):base(context)
        {
            
        }

        public List<AppUser> GetByName(string name)
        {
            return [..context.AppUsers.Where(u => u.FName == name)];
        }

    }
}
