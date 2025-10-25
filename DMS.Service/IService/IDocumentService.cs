using DMS.Domain.Models;
using DMS.Service.ModelViews.DocumentViews;

namespace DMS.Service.IService
{
    public interface IDocumentService
    {
        Task<DocumentIndexViewModel> GetDocumentsByFolderIdAsync(string patentFolderId);
        Task DeleteDocumentAsync(string docId);
        Task<bool> TrashDocumentAsync(string docId);
        Task<bool> StarDocumentAsync(string docId, bool isStar);
        Task<DocumentFileResultViewModel?> GetFileToDownloadAsync
            (string docId, string wwwroot);
        Task<bool> UploadDocumentAsync(DocumentUploadViewModel model, string wwwroot);
        Task<Document?> GetDocumentByIdAsync(string docId);
        Task<bool> EditDocumentAsync(DocumentUploadViewModel model, string wwwroot);
        Task<DocumentDetailsViewModel?> GetDocumentDetailsAsync(string id);
        Task<DocumentIndexViewModel> GetDocumentsByFolderIdWithPaginationAsync
            (string patentFolderId, string? searchName, int pageNum, int pageSize,
            string sortField, string sortOrder);
    }
}
