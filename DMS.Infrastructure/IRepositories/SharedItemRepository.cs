using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public class SharedItemRepository: 
        GenericRepository<SharedItem>, ISharedItemRepository
    {
        public SharedItemRepository(DMSContext context): base(context) { }
    }
}
