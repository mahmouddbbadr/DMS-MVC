using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Folder
{
    public class FolderListItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsStarred { get; set; }
        public DateTime AddedAt { get; set; }
        public long TotalSize { get; set; }
        public int ItemCount { get; set; }
        //public string? ParentFolderId { get; set; }
    }
}
