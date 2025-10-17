using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;

namespace DMS.Infrastructure.IRepositories
{
    public interface IFolderRepository: ISortSearch<Folder>, IRepository<Folder>
    {
        public Task<List<Folder>> GetFoldersByFolderIdAsync(string? parentFolderId);
    }
}
