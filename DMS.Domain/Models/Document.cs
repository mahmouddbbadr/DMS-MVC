using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime UploadedAt { get; set; }
        public int Size { get; set; }
        public Guid FolderId { get; set; }
        public Folder? Folder { get; set; }
    }
}
