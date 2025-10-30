using DMS.Domain.ENums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Shared
{
    public class ShareViewModel
    {
        public string? SharedItemId { get; set; }

        public string? FolderId { get; set; }

        public string? DocumentId { get; set; }
        [Required(ErrorMessage = "Please enter the email of the user to share with.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string SharedWithUserEmail { get; set; } = string.Empty;
        public PermissionLevel Permission { get; set; } = PermissionLevel.Read;
        public string? SharedByUserId { get; set; }
       
        public DateTime SharedDate { get; set; } = DateTime.UtcNow;
    }
}
