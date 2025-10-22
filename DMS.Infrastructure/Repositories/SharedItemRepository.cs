using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repository
{
    public class SharedItemRepository: 
        GenericRepository<SharedItem>, ISharedItemRepository
    {
        public SharedItemRepository(DMSContext context): base(context) { }
        
    }
}
