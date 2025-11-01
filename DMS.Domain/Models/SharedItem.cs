using DMS.Domain.ENums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Models
{
    public class SharedItem 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public PermissionLevel PermissionLevel { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public string? DocumentId { get; set; }
        public virtual Document? Document { get; set; }
        public string? FolderId { get; set; }
        public virtual Folder? Folder { get; set; }
        public string SharedWithUserId { get; set; } ///////////////receiver
        public string SharedByUserId { get; set; }  ///////////// Sender
        public virtual AppUser? SharedWithUser { get; set; }
        public virtual AppUser? SharedByUser { get; set; }
        
    }
}
