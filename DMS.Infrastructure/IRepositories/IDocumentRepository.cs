using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;

namespace DMS.Infrastructure.IRepositories
{
    public interface IDocumentRepository: ISortSearch<Document>, IRepository<Document>
    {
        Task<long> GetTotalStorageAsync();
        Task<long> GetTotalStorageByUserAsync(string userId);
        Task<List<Document>> GetDocumentsByFolderIdAsync(string folderId, string userId);
        IQueryable<Document> SearchDocumentByFolderAsQueryable(string folderId, string userId, string searchName);
        IQueryable<Document> GetDocumentsByFolderIdAsQueryable(string folderId, string userId);
        IQueryable<Document> SortedBySize();
        IQueryable<Document> SortedBySizeDesc();
    }
}
