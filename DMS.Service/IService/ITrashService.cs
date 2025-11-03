using DMS.Domain.Models;
using DMS.Service.ModelViews.TrashViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.IService
{
    public interface ITrashService
    {
        Task<TrashViewModel> GetTrashOverviewAsync
            (TrashFilterViewModel filterModel);
        Task<TrashFoldersViewModel> GetTrashedFolderAsync
            (TrashFilterViewModel filterModel);
        Task<TrashDocumentsViewModel> GetTrashedDocumentAsync
            (TrashFilterViewModel filterModel);
        Task<Folder?> GetFolderByIdAsync(string id, string userId);
        Task<(bool Success, string Message)> RestoreDocumentAsync(string documentId, string userId);
        Task<(bool Success, string Message)> RestoreFolderAsync(string folderId, string userId);
        Task<bool> DeleteFolderAsync(string folderId, string userId);
        Task<bool> DeleteDocumentAsync(string documentId, string userId, string wwwroot);
    }
}
