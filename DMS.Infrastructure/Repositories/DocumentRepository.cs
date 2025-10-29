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
        public async Task<List<Document>> GetDocumentsByFolderIdAsync(string folderId, string userId)
        {
            return await _context.Documents
                .Where(d => d.FolderId == folderId && d.OwnerId == userId)
                .ToListAsync();
        }
        public IQueryable<Document> GetDocumentsByFolderIdAsQueryable(string folderId, string userId)
        {
            return _context.Documents
                .Where(d => d.FolderId == folderId && d.OwnerId == userId)
                .AsQueryable();
        }
        public IQueryable<Document> SearchDocumentByFolderAsQueryable
            (string folderId, string userId, string searchName)
        {
            return GetDocumentsByFolderIdAsQueryable(folderId, userId)
                .Where(d => EF.Functions.Like(d.Name, $"%{searchName}%"));
        }

        public IQueryable<Document> SortedBySize() => _context.Documents.OrderBy(f => f.Size);
        public IQueryable<Document> SortedBySizeDesc() => _context.Documents.OrderByDescending(f => f.Size);
    }
}
