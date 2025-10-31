using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.TrashViews
{
    public class TrashedDocumentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? FolderId { get; set; }
        public string? FolderName { get; set; }  // Helpful to show where it was deleted from
        public bool FolderIsDeleted { get; set; }
    }
}
