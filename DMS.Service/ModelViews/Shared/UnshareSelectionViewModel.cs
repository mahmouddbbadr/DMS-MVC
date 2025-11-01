using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Shared
{
    public class UnshareSelectionViewModel
    {
        [Required]
        public string ItemId { get; set; }

        [Required]
        public string Type { get; set; } 
        public string ItemName { get; set; }

        public List<string> SelectedUserIds { get; set; } = new List<string>();

        public List<SharedUserViewModel> SharedUsers { get; set; } = new List<SharedUserViewModel>();

    }
}
