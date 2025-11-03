using DMS.Domain.Models;
using DMS.Service.ModelViews.Folder;

namespace DMS.Service.IService
{
    public interface IFolderService
    {
        Task<FolderIndexViewModel> GetFoldersWithPaginationAsync
            (FolderQueryViewModel modelQuery);
        Task<bool> StarFolderAsync(string folId, string userId, bool isStar);
        Task<bool> TrashFolderAsync(string folId, string userId);
        Task<CreateFolderResult> CreateAsync(FolderCreateViewModel model);
        Task<Folder?> GetFolderAsync(string folId, string userId);
        Task<bool> RenameFolderAsync(FolderEditViewModel model, string userId);
    }
}
