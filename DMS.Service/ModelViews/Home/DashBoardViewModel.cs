using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews
{
    public class DashBoardViewModel
    {
        public int TotalDocuments { get; set; }
        public int? TotalFolders { get; set; }
        public long TotalStorage { get; set; }
        public int SharedWithMe { get; set; }
        public int SharedByMe { get; set; }

        // for Admin 
        public int TotalUsers { get; set; }
        public int BlockedUsers { get; set; }
    }
}
