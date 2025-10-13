using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;

namespace DMS.Infrastructure.IRepositories
{
    public class FolderRepository : GenericRepository<Folder>, IFolderRepository
    {
        public FolderRepository(DMSContext context):base(context)
        {
            
        }
    }
}
