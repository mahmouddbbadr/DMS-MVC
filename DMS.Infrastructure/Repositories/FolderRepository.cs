using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;

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


    }
}
