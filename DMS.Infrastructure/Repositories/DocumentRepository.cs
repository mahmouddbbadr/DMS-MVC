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

        public async Task<long> GetTotalStorageAsync()
        {
            var totalMb = await _context.Documents
                .SumAsync(d => d.Size);

            //return Math.Round((double)totalMb, 3);
            return totalMb;
        }

        public async Task<long> GetTotalStorageByUserAsync(string userId)
        {
            var totalMb = await _context.Documents
                .Where(d => d.Folder.OwnerId == userId)
                .SumAsync(d => d.Size); // Convert bytes to bytes
            
            return totalMb;
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


        // For Sharing
        public IQueryable<Document> GetDocumentsBySharedFolderIdAsQueryable(string folderId)
        {
            return _context.Documents
                .Where(d => d.FolderId == folderId)
                .AsQueryable();
        }

        public IQueryable<Document> SearchDocumentsBySharedFolderAsQueryable
            (string folderId, string searchName)
        {
            return GetDocumentsBySharedFolderIdAsQueryable(folderId)
                .Where(d => EF.Functions.Like(d.Name, $"%{searchName}%"));
        }

        public async Task<bool> DocumentNameExistAsync(string userId, string folderId, string newName, string? execludeId = null)
        {
            bool exists = await _context.Documents
            .Where(d => d.OwnerId == userId
                        && d.FolderId == folderId
                        && d.Name.ToLower() == newName.ToLower())
            .Where(d => execludeId == null || d.Id != execludeId)
            .AnyAsync();

            return exists;
        }

    }
}
