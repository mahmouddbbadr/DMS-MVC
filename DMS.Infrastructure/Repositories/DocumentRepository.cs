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
    }
}
