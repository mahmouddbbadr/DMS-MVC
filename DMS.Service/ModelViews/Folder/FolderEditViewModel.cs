using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Folder
{
    public class FolderEditViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Folder name is required.")]
        [StringLength(50, ErrorMessage = "Folder name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;
    }

}
