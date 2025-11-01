using DMS.Service.ModelViews.DocumentViews;
using DMS.Service.ModelViews.Shared;
namespace DMS.Service.IService
{
    public interface ISharingService
    {
        Task ShareFolderAsync(ShareViewModel model);
        
        Task ShareDocumentAsync(ShareViewModel model);
        Task UnshareAsync(string sharedItemId);
        Task<SharedIndexViewModel> GetSharedWithMeAsync(string userId,
            string search = "",
            string sortOrder = "dateDesc",int page = 1,int pageSize = 5);
        Task<SharedIndexViewModel> GetSharedByMeAsync(string userId,
            string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 5);
        Task<int> UnshareAllAsync(string itemId, string itemType, string currentUserId);
        Task<UnshareSelectionViewModel> GetSharedUsersAsync(string itemId, string type, string currentUserId);
        Task<int> UnshareWithUsersAsync(string itemId, string type, List<string> userIds, string currentUserId);
        Task<bool> UnshareWithUserAsync(string itemId, string type, string userId, string currentUserId);
        Task<string> GetUserEmailAsync(string userId);
        Task<List<string>> GetUserEmailsAsync(List<string> userIds);
        Task<bool> IsAuthorizedSharedFolderAsync(string fId, string userId);
        Task<DocumentIndexViewModel> GetDocumentsBySharedFolderIdWithPaginationAsync
            (FolderSharedViewModel modelQuery);
    }

}
