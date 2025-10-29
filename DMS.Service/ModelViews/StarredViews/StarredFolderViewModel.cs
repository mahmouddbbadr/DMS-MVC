using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.StarredViews
{
    public class StarredFolderViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ItemCount { get; set; }
        public bool IsStarred { get; set; }
        public long TotalSize { get; set; }
        public DateTime? AddedAt { get; set; }
        public string? ParentFolderName { get; set; }
    }
}
