using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.UnitOfWorks;

namespace DMS.Service.Service
{
    public class DashBoardService
    {
        private readonly DMSContext context;
        private readonly UnitOfWork _unit;

        public DashBoardService(UnitOfWork unit,DMSContext context)
        {
            this.context = context;
            this._unit = unit;
        }

        public async Task<DashBoardViewModel> GetAdminStats(string adminId)
        {
            return new DashBoardViewModel
            {
                TotalDocuments = await _unit.DocumentRepository.GetCountAsync(),
                TotalFolders = await _unit.FolderRepository.GetCountAsync(),
                TotalStorage = context.Documents.Sum(d => (double?)d.Size) ?? 0,
                SharedByMe = await _unit.SharedItemRepository.GetSharedByMeCountAsync(adminId),
                SharedWithMe = await _unit.SharedItemRepository.GetSharedWithMeCountAsync(adminId),
                TotalUsers = await _unit.AppUserRepository.GetCountAsync(),
                BlockedUsers = await _unit.AppUserRepository.GetCountAsync(),
            };
        }

        public async Task<DashBoardViewModel> GetUserStats(string userId)
        {
            return new DashBoardViewModel
            {
                TotalDocuments = context.Documents.Count(d => d.Folder.OwnerId == userId),
                //TotalFolders = context.Folders.Count(f => f.OwnerId == userId),
                TotalFolders = _unit.AppUserRepository.GetFolderCountAsync(userId),
                TotalStorage = context.Documents
                                     .Where(d => d.Folder.OwnerId == userId)
                                     .Sum(d => (double?)d.Size) ?? 0,
                SharedByMe = await _unit.SharedItemRepository.GetSharedByMeCountAsync(userId),
                SharedWithMe = await _unit.SharedItemRepository.GetSharedWithMeCountAsync(userId)
            };
        }

    }
}
