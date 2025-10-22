using DMS.Domain.ENums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.SharedViewModel
{
    public class SharedItemViewModel
    {
        public string Id { get; set; }=string.Empty ;
        public string? FolderName { get; set; }
        public string? DocumentName { get; set; }
        public string SharedByUserName { get; set; } = string.Empty;
        public string SharedWithUserName { get; set; } = string.Empty;
        public PermissionLevel Permission { get; set; } = PermissionLevel.Read;
        public DateTime SharedDate { get; set; }
        public bool IsSharedByMe { get; set; }

    }
}
