using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.IRepositories
{
    public class FolderRepository : SortSearch<Folder>, IFolderRepository
    {
        public FolderRepository(DMSContext context):base(context)
        {
            
        }

 
    }
}
