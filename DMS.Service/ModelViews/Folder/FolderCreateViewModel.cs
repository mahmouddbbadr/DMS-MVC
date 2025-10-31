using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Folder
{
    public class FolderCreateViewModel
    {
        public string? Id { get; set; }

        [ValidateNever]
        public string OwnerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Folder name is required.")]
        [StringLength(50, ErrorMessage = "Folder name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
