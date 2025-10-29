using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.StarredViews
{
    public class StarredViewModel
    {
        public StarFoldersViewModel StarFolders { get; set; } = new();
        public StarDocumentsViewModel StarDocuments { get; set; } = new();
    }
}
