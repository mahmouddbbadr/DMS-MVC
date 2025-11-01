using DMS.Domain.Models;
using DMS.Service.ModelViews.StarredViews;

namespace DMS.Service.IService
{
    public interface IStarredService
    {
        Task<StarredViewModel> GetStarOverviewAsync(StarredFilterViewModel model);
        Task<StarFoldersViewModel> GetStarFoldersAsync(StarredFilterViewModel model);
        Task<StarDocumentsViewModel> GetStarDocumentsAsync(StarredFilterViewModel model);
        bool UnStar<T>(T item) where T : IBaseEntity;
        Task<Folder?> GetFolderByIdAsync(string id, string userId);
        Task<Document?> GetDocumentByIdAsync(string id, string userId);
    }
}
