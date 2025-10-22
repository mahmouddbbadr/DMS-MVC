using DMS.Service.ModelViews.SharedViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.IService
{
    public interface ISharingService
    {
        Task ShareFolderAsync(ShareViewModel model);
        
        Task ShareDocumentAsync(ShareViewModel model);
        Task UnshareAsync(string sharedItemId);
        Task<List<SharedItemViewModel>> GetSharedWithMeAsync(string userId,
            string search = "",
            string sortOrder = "dateDesc",int page = 1,int pageSize = 5);
        Task<List<SharedItemViewModel>> GetSharedByMeAsync(string userId,
            string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 5);

    }
}
