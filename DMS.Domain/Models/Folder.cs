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
        public ICollection<Document>? Documents { get; set; } = new HashSet<Document>();
        public string OwnerId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public string FolderId { get; set; }
        public virtual ICollection<Folder>? Folders { get; set;} = new HashSet<Folder>();
        public virtual Folder? ParentFolder { get; set; }
        public virtual ICollection<SharedItem>? SharedFolder { get; set; } = new HashSet<SharedItem>();
    }
}
