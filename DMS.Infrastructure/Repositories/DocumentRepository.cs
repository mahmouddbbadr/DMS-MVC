using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repository
{
    public class DocumentRepository : SortSearch<Document>, IDocumentRepository
    {
        public DocumentRepository(DMSContext context):base(context)
        {
            
        }
        public async Task<List<Document>> GetDocumentsByFolderIdAsync(string folderId)
        {
            return await _context.Documents
                .Where(d => d.FolderId == folderId)
                .ToListAsync();
        }
        public IQueryable<Document> GetDocumentsByFolderIdAsQueryable(string folderId)
        {
            return _context.Documents
                .Where(d => d.FolderId == folderId)
                .AsQueryable();
        }
        public IQueryable<Document> SearchDocumentByFolderAsQueryable
            (string folderId, string searchName)
        {
            return GetDocumentsByFolderIdAsQueryable(folderId)
                .Where(d => EF.Functions.Like(d.Name, $"%{searchName}%"));
        }

        public IQueryable<Document> SortedBySize() => _context.Documents.OrderBy(f => f.Size);
        public IQueryable<Document> SortedBySizeDesc() => _context.Documents.OrderByDescending(f => f.Size);
    }
}
