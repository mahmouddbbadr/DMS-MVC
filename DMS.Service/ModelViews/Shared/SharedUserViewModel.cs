using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Shared
{
    public class SharedUserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime SharedDate { get; set; }
        public string FormattedSharedDate => SharedDate.ToString("dd MMM yyyy 'at' HH:mm");
    }
}
