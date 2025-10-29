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

    }
}
