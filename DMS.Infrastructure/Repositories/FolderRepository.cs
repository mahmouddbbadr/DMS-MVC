using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repository
{
    public class FolderRepository : SortSearch<Folder>, IFolderRepository
    {
        public FolderRepository(DMSContext context):base(context)
        {
            
        }
        public async Task<List<Folder>> GetFoldersByFolderIdAsync(string? parentFolderId)
        {
            IQueryable<Folder> query = _context.Folders;

            if (parentFolderId != null)
            {
                 query = query.Where(f => f.ParentFolderId == parentFolderId);
            }
            return await query.ToListAsync();
        }

        public long GetTotalSize(string id)
        {
            long total = 0;
            Folder? folder = _context.Folders.SingleOrDefault(f => f.Id == id);
            if(folder != null)
            {
                List<Document>? docList = folder.Documents?.ToList();

                if(docList != null)
                {
                    foreach (var doc in docList)
                    {
                        total += doc.Size;
                    }
                }
            }
            return total;
        }

        public async Task<Folder?> GetFolderByIdAuthorizeAsync(string folderId, string userId)
        {
            return await _context.Folders
                .SingleOrDefaultAsync(f => f.Id == folderId && f.OwnerId == userId);
        }

        // 
        public IQueryable<Folder> GetDeletedFolders(string userId)
        {
            return _context.Folders
                .IgnoreQueryFilters()
                .Where(f => f.IsDeleted && f.OwnerId == userId)
                .Include(f => f.Documents);
        }
        public async Task<Folder?> GetDeletedFolderByIdAsync(string id, string userId)
        {
            return await GetDeletedFolders(userId)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
        public IQueryable<Folder> GetFoldersAsQueryable(string? parentFolderId, string userId)
        {
            if(parentFolderId == null)
            {
                return _context.Folders
                    .Where(f => f.ParentFolderId == null && f.OwnerId == userId)
                    .AsQueryable();
            }
            return _context.Folders
                .Where(f => f.ParentFolderId == parentFolderId && f.OwnerId == userId)
                .AsQueryable();
        }
        public IQueryable<Folder> SearchFoldersAsQueryable
            (string? parentFolderId, string userId, string searchName)
        {
            return GetFoldersAsQueryable(parentFolderId, userId)
                .Where(d => EF.Functions.Like(d.Name, $"%{searchName}%"));
        }
        public long FolderTotalSize(string folderId)
        {
            long totalSize = 0;
            var folder = _context.Folders
                .Include(f => f.Documents)
                .SingleOrDefault(f => f.Id == folderId);
            if (folder != null && folder.Documents != null)
            {
                foreach (var document in folder.Documents)
                {
                    totalSize += document.Size;
                }
            }
            return totalSize;
        }

        public async Task<bool> FolderNameExistAsync(string userId, string newName, string? execludeId = null)
        {
            var exist = await _context.Folders
                .Where(f => f.OwnerId == userId && f.Name == newName)
                .Where(f => execludeId == null || f.Id != execludeId)
                .AnyAsync();

            return exist;
        }
    }
}
