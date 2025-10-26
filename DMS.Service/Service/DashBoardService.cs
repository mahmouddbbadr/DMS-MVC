using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews;


namespace DMS.Service.Service
{
    public class DashBoardService : IDashBoardService
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
                TotalStorage = await _unit.DocumentRepository.GetTotalStorageAsync(),
                SharedByMe = await _unit.SharedItemRepository.GetSharedByMeCountAsync(adminId),
                SharedWithMe = await _unit.SharedItemRepository.GetSharedWithMeCountAsync(adminId),
                TotalUsers = await _unit.AppUserRepository.GetCountAsync(),
                BlockedUsers = (await _unit.AppUserRepository.GetBlockedUsersAsync()).Count,
            };
        }

        public async Task<DashBoardViewModel> GetUserStats(string userId)
        {
            return new DashBoardViewModel
            {
                TotalDocuments = _unit.AppUserRepository.GetDocumentsCountAsync(userId),
                TotalFolders = _unit.AppUserRepository.GetFoldersCountAsync(userId),
                TotalStorage = await _unit.DocumentRepository.GetTotalStorageByUserAsync(userId),
                SharedByMe = await _unit.SharedItemRepository.GetSharedByMeCountAsync(userId),
                SharedWithMe = await _unit.SharedItemRepository.GetSharedWithMeCountAsync(userId)
            };
        }

    }
}
