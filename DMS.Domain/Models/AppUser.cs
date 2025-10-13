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
        public string? Address { get; set; }
        public bool IsLocked { get; set; }
        public string WorkSpaceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Folder>? Folders { get; set; } = new HashSet<Folder>();
    }
}
