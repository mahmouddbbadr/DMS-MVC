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

 
    }
}
