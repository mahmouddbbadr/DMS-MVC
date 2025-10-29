using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Models
{
    public class Document : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsStarred { get; set; } = false;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public int Size { get; set; }
        public string OwnerId { get; set; }
        public virtual AppUser? Owner { get; set; }
        public string FolderId { get; set; }
        public virtual Folder? Folder { get; set; }
        public virtual ICollection<SharedItem>? SharedDocument { get; set; } = new HashSet<SharedItem>();

    }
}
