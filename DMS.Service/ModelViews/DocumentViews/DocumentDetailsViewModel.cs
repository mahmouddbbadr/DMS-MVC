using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.DocumentViews
{
    public class DocumentDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public bool IsStarred { get; set; }
        public DateTime AddedAt { get; set; }
        public int Size { get; set; }
        public string FolderId { get; set; }
        public string FolderName { get; set; }
    }
}
