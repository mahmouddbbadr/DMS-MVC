using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.TrashViews
{
    public class TrashFoldersViewModel
    {
        public PagedResult<TrashedFolderViewModel> TrashedFolders { get; set; } = new();
    }
}
