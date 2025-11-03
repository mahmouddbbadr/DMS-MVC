using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.DocumentViews
{
    public class DocumentDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public bool IsStarred { get; set; }
        public DateTime AddedAt { get; set; }
        public int Size { get; set; }
        public string FolderId { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
    }

}



