using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;

namespace DMS.Infrastructure.IRepositories
{
    public interface IDocumentRepository: ISortSearch<Document>, IRepository<Document>
    {
        public Task<List<Document>> GetDocumentsByFolderIdAsync(string folderId);
    }
}
