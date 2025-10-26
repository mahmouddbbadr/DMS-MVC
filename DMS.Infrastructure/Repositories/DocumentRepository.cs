using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<double> GetTotalStorageAsync()
        {
            return await _context.Documents.SumAsync(d => (double?)d.Size) ?? 0;
        }

        public async Task<double> GetTotalStorageByUserAsync(string userId)
        {
            return await _context.Documents
                .Where(d => d.Folder.OwnerId == userId)
                .SumAsync(d => (double?)d.Size) ?? 0;
        }

    }
}
