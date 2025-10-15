using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Models
{
    public interface IBaseEntity
    {
        public string Name { get; set; }
        public DateTime AddedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsStarred { get; set; }
    }
}
