using DMS.Infrastructure.IRepositorys;
using Microsoft.AspNetCore.Identity;

namespace DMS.Infrastructure.IRepositories
{
    public interface IRoleRepository: IRepository<IdentityRole>
    {
        public Task<List<IdentityRole>> GetByNameAsync(string name);
    }
}
