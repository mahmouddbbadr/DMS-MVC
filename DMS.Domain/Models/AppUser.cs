using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Models
{
    public class AppUser: IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public bool IsLocked { get; set; }
        public long? TotalSize { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Folder>? Folders { get; set; } = new HashSet<Folder>();
        public virtual ICollection<Document>? Documents { get; set; } = new HashSet<Document>();
        public virtual ICollection<SharedItem>? ReceivedSharedItems { get; set; } = new HashSet<SharedItem>();
        public virtual ICollection<SharedItem>? SharedItems { get; set; } = new HashSet<SharedItem>();
    }
}
