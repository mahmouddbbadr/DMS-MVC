using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Models
{
    public class Folder : IBaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsStarred { get; set; } = false;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public virtual ICollection<Document>? Documents { get; set; } = new HashSet<Document>();
        public string OwnerId { get; set; }
        public virtual AppUser? Owner { get; set; }
        public string? ParentFolderId { get; set; }
        public virtual Folder? ParentFolder { get; set; }
        public virtual ICollection<Folder>? SubFolders { get; set;} = new HashSet<Folder>();
        public virtual ICollection<SharedItem>? SharedFolders { get; set; } = new HashSet<SharedItem>();
    }
}
