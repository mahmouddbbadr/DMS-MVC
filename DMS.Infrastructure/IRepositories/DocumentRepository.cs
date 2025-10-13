using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;

namespace DMS.Infrastructure.IRepositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(DMSContext context):base(context)
        {
            
        }
    }
}
