using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;

namespace DMS.Infrastructure.IRepositories
{
    public interface IFolderRepository: ISortSearch<Folder>, IRepository<Folder>
    {
        Task<List<Folder>> GetFoldersByFolderIdAsync(string? parentFolderId);
        long GetTotalSize(string id);
    }
}
