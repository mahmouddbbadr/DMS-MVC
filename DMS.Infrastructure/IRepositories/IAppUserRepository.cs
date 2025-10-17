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
        public Task<List<AppUser>> GetByNameAsync(string name);
    }
}
