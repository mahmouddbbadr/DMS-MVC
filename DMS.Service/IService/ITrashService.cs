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
            (string? searchName, int pageNum, int pageSize,
            string sortField, string sortOrder);
        Task<TrashFoldersViewModel>GetTrashedFolderAsync
            (string? searchName, int pageNum, int pageSize,
            string sortField, string sortOrder);
        Task<TrashDocumentsViewModel> GetTrashedDocumentAsync
            (string? searchName, int pageNum, int pageSize,
            string sortField, string sortOrder);
        Task<Folder?> GetFolderByIdAsync(string id);
        Task<Document?> GetDocumentByIdAsync(string id);
        Task<bool> RestoreFolderAsync(Folder folder);
        Task<bool> RestoreDocumentAsync(Document document);
        Task<bool> DeleteFolderAsync(string folderId);
        Task<bool> DeleteDocumentAsync(string documentId);
    }
}
