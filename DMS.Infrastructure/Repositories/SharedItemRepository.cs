using DMS.Domain.ENums;
using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repository
{
    public class SharedItemRepository: 
        GenericRepository<SharedItem>, ISharedItemRepository
    {
        public SharedItemRepository(DMSContext context): base(context) { }

    //    public async Task<List<SharedItem>> GetSharedByMeQueryAsync(
    //string userId,
    //string search = "",
    //string sortOrder = "dateDesc",
    //int page = 1,
    //int pageSize = 5)
    //    {
    //        var query = GetAllAsQueryable();

    //        if (!string.IsNullOrEmpty(search))
    //        {
    //            search = search.Trim().ToLower();
    //            query = query.Where(s =>
    //                (s.Document != null && s.Document.Name.ToLower().Contains(search)) ||
    //                (s.Folder != null && s.Folder.Name.ToLower().Contains(search)) ||
    //                (s.SharedWithUser != null && s.SharedWithUser.UserName.ToLower().Contains(search))
    //            );
    //        }

    //        query = sortOrder switch
    //        {
    //            "nameAsc" => query.OrderBy(s => s.Document!.Name ?? s.Folder!.Name),
    //            "nameDesc" => query.OrderByDescending(s => s.Document!.Name ?? s.Folder!.Name),
    //            "dateAsc" => query.OrderBy(s => s.AddedAt),
    //            "dateDesc" => query.OrderByDescending(s => s.AddedAt),
    //            _ => query.OrderByDescending(s => s.AddedAt)
    //        };

    //        return await GetAllWithPaginationAsync(query, page, pageSize);
    //    }
        public async Task<bool> AnyAsync(Expression<Func<SharedItem, bool>> predicate)
        {
            return await _context.SharedItems.AnyAsync(predicate);
        }
        public async Task<List<SharedItem>> GetSharedItemsByUserAndItemAsync(string itemId, string itemType, string userId)
        {
            if (string.IsNullOrWhiteSpace(itemId) || string.IsNullOrWhiteSpace(userId))
                return new List<SharedItem>(); 

            itemType = itemType?.ToLower() ?? "";
            IQueryable<SharedItem> query = _context.SharedItems
                .Include(s => s.SharedWithUser)  
                .Include(s => s.Document)        
                .Include(s => s.Folder)          
                .Where(s => s.SharedByUserId == userId);

            if (itemType.Equals("document", StringComparison.OrdinalIgnoreCase))
                query = query.Where(s => s.DocumentId == itemId);
            else if (itemType.Equals("folder", StringComparison.OrdinalIgnoreCase))
                query = query.Where(s => s.FolderId == itemId);

            return await query.ToListAsync();
        }

        public async Task<SharedItem> FirstOrDefaultAsync(Expression<Func<SharedItem, bool>> predicate)
        {
            return await _context.SharedItems
                .FirstOrDefaultAsync(predicate);
                
        }

        public async Task<int> GetSharedWithMeCountAsync(string id)
        {
            return await _context.SharedItems.Where(s => s.SharedWithUserId == id).CountAsync();
        }
        public async Task<int> GetSharedByMeCountAsync(string id)
        {
            return await _context.SharedItems.Where(s => s.SharedByUserId == id).CountAsync();
        }

        //////////////////////////

        public IQueryable<SharedItem> GetSharedByMeQuery(
            string userId,
            string search,
            string sortOrder,
            int page,
            int pageSize)
        {
            return ExecuteSharedQuery(SharedFilterType.SharedByMe, userId, search, sortOrder, page, pageSize);
        }

        public IQueryable<SharedItem> GetSharedWithMeQuery(
            string userId,
            string search,
            string sortOrder,
            int page,
            int pageSize)
        {
            return ExecuteSharedQuery(SharedFilterType.SharedWithMe, userId, search, sortOrder, page, pageSize);
        }

        private IQueryable<SharedItem> BuildSharedQuery(
            IQueryable<SharedItem> query,
            string userId,
            SharedFilterType filterType,
            string search)
        {
            // Filter Main Condition
            query = filterType switch
            {
                SharedFilterType.SharedByMe => query.Where(s => s.SharedByUserId == userId),
                SharedFilterType.SharedWithMe => query.Where(s => s.SharedWithUserId == userId),
                _ => query
            };

            
            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(s =>
                    (s.Document != null && s.Document.Name.ToLower().Contains(search)) ||
                    (s.Folder != null && s.Folder.Name.ToLower().Contains(search)) ||
                    (filterType == SharedFilterType.SharedByMe
                        ? (s.SharedWithUser != null && s.SharedWithUser.UserName.ToLower().Contains(search))
                        : (s.SharedByUser != null && s.SharedByUser.UserName.ToLower().Contains(search)))
                );
            }

            return query;
        }


        private IQueryable<SharedItem> ExecuteSharedQuery(
            SharedFilterType filterType,
            string userId,
            string search,
            string sortOrder,
            int page,
            int pageSize)
        {
            var query = GetAllAsQueryable();

            query = BuildSharedQuery(query, userId, filterType, search);

            // Sorting
            query = sortOrder switch
            {
                "nameAsc" => query.OrderBy(s => s.Document!.Name ?? s.Folder!.Name),
                "nameDesc" => query.OrderByDescending(s => s.Document!.Name ?? s.Folder!.Name),
                "dateAsc" => query.OrderBy(s => s.AddedAt),
                _ => query.OrderByDescending(s => s.AddedAt)
            };

            query = filterType switch
            {
                SharedFilterType.SharedByMe => query.Where(s => s.SharedByUserId == userId)
                    .GroupBy(s => new { s.DocumentId, s.FolderId })
                    .Select(g => g.First()),
                SharedFilterType.SharedWithMe => query.Where(s => s.SharedWithUserId == userId)
                    .GroupBy(s => new { s.DocumentId, s.FolderId })
                    .Select(g => g.First()),
                _ => query
            };

            //return await GetAllWithPaginationAsync(query, page, pageSize);
            return query;
        }


        public async Task<SharedItem?> GetSharedFolderByIdAuthorizeAsync(string folderId, string userId)
        {
            return await _context.SharedItems
                .SingleOrDefaultAsync(si => si.FolderId == folderId && si.SharedWithUserId == userId);
        }
    }
}
