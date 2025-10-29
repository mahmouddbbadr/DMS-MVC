using DMS.Service.ModelViews.TrashViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.StarredViews
{
    public class StarFoldersViewModel
    {
        public PagedResult<StarredFolderViewModel> StarredFolder { get; set; } = new();
    }
}
