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
        public async Task<List<SharedItem>> GetSharedByMeQueryAsync(
    string userId,
    string search = "",
    string sortOrder = "dateDesc",
    int page = 1,
    int pageSize = 5)
        {
            var query = GetAllAsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(s =>
                    (s.Document != null && s.Document.Name.ToLower().Contains(search)) ||
                    (s.Folder != null && s.Folder.Name.ToLower().Contains(search)) ||
                    (s.SharedWithUser != null && s.SharedWithUser.UserName.ToLower().Contains(search))
                );
            }

            query = sortOrder switch
            {
                "nameAsc" => query.OrderBy(s => s.Document!.Name ?? s.Folder!.Name),
                "nameDesc" => query.OrderByDescending(s => s.Document!.Name ?? s.Folder!.Name),
                "dateAsc" => query.OrderBy(s => s.AddedAt),
                "dateDesc" => query.OrderByDescending(s => s.AddedAt),
                _ => query.OrderByDescending(s => s.AddedAt)
            };

            return await GetAllWithPaginationAsync(query, page, pageSize);
        }

    }
}
