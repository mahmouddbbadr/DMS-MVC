using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Models
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Document>? Documents { get; set; } = new HashSet<Document>();
        public string AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public Guid FolderId { get; set; }
        public virtual ICollection<Folder>? Folders { get; set;} = new HashSet<Folder>();
        public virtual Folder? ParentFolder { get; set; }
    }
}
