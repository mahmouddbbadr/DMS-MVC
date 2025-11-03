using DMS.Domain.Models;
using DMS.Service.ModelViews.DocumentViews;

namespace DMS.Service.IService
{
    public interface IDocumentService
    {
        Task<DocumentIndexViewModel> GetDocumentsByFolderIdAsync(string patentFolderId, string userId);
        Task DeleteDocumentAsync(string docId, string userId);
        Task<bool> TrashDocumentAsync(string docId, string userId);
        Task<bool> StarDocumentAsync(string docId, string userId, bool isStar);
        Task<DocumentFileResultViewModel?> GetFileToDownloadAsync
            (string docId, string userId, string wwwroot);
        Task<UploadResult> UploadDocumentAsync(DocumentUploadViewModel model, string wwwroot);
        Task<Document?> GetDocumentByIdAsync(string docId, string userId);
        Task<bool> EditDocumentAsync(DocumentUploadViewModel model, string wwwroot);
        Task<DocumentDetailsViewModel?> GetDocumentDetailsAsync(string id, string userId);
        Task<DocumentIndexViewModel> GetDocumentsByFolderIdWithPaginationAsync
            (DocumentQueryViewModel modelQuery);
        Task<DocumentUploadViewModel?> SetEditDocumentAsync
            (DocumentEditViewModel model, string userId);
        Task<bool> IsAuthorizedFolderAsync(string fId, string userId);
        Task<bool> EditDocumentModelAsync(DocumentEditModelViewModel model, string wwwroot);
    }
}
