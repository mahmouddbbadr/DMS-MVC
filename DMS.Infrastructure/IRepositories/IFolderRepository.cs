using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;

namespace DMS.Infrastructure.IRepositories
{
    public interface IFolderRepository: ISortSearch<Folder>, IRepository<Folder>
    {
        Task<List<Folder>> GetFoldersByFolderIdAsync(string? parentFolderId);
        long GetTotalSize(string id);
        Task<Folder?> GetFolderByIdAuthorizeAsync(string folderId, string userId);

        //
        IQueryable<Folder> GetDeletedFolders(string userId);
        Task<Folder?> GetDeletedFolderByIdAsync(string id, string userId);
        IQueryable<Folder> GetFoldersAsQueryable(string? parentFolderId, string userId);
        IQueryable<Folder> SearchFoldersAsQueryable
            (string? parentFolderId, string userId, string searchName);
        long FolderTotalSize(string folderId);
        Task<bool> FolderNameExistAsync(string userId, string newName, string? execludeId = null);
    }
}
