using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;

namespace DMS.Infrastructure.Repository
{
    public class DocumentRepository : SortSearch<Document>, IDocumentRepository
    {
        public DocumentRepository(DMSContext context):base(context)
        {
            
        }
    }
}
