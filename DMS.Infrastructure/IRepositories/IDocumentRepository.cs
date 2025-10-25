using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;

namespace DMS.Infrastructure.IRepositories
{
    public interface IDocumentRepository: ISortSearch<Document>, IRepository<Document>
    {
        Task<List<Document>> GetDocumentsByFolderIdAsync(string folderId);
        IQueryable<Document> SearchDocumentByFolderAsQueryable(string folderId, string searchName);
        IQueryable<Document> GetDocumentsByFolderIdAsQueryable(string folderId);
        IQueryable<Document> SortedBySize();
        IQueryable<Document> SortedBySizeDesc();
    }
}
