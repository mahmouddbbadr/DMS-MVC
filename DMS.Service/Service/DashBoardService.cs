using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;

namespace DMS.Service.Service
{
    public class DashBoardService
    {
        private readonly DMSContext context;

        public DashBoardService(DMSContext context)
        {
            this.context = context;
        }

        public DashBoardViewModel GetAdminStats()
        {
            return new DashBoardViewModel
            {
                TotalDocuments = context.Documents.Count(),
                TotalFolders = context.Folders.Count(),
                TotalStorage = context.Documents.Sum(d => (double?)d.Size) ?? 0,
                SharedByMe = context.SharedItems.Count(),
                SharedWithMe = context.SharedItems.Count(),
                TotalUsers = context.Users.Count(),
                BlockedUsers = context.Users.Count(u => u.IsLocked)
            };
        }

        public DashBoardViewModel GetUserStats(string userId)
        {
            return new DashBoardViewModel
            {
                TotalDocuments = context.Documents.Count(d => d.Folder.OwnerId == userId),
                TotalFolders = context.Folders.Count(f => f.OwnerId == userId),
                TotalStorage = context.Documents
                                     .Where(d => d.Folder.OwnerId == userId)
                                     .Sum(d => (double?)d.Size) ?? 0,
                SharedByMe = context.SharedItems.Count(s => s.SharedByUserId == userId),
                SharedWithMe = context.SharedItems.Count(s => s.SharedWithUserId == userId)
            };
        }

    }
}
