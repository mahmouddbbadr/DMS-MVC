using DMS.Domain.ENums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Shared
{
    public class FolderSharedViewModel
    {
        [ValidateNever]
        public string OwnerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Folder ID is required.")]
        public string FolderId { get; set; } = string.Empty;
        public string? SearchName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNum { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 4;
        public string SortField { get; set; } = "AddedAt";

        [RegularExpression("asc|desc", ErrorMessage = "Sort order must be 'asc' or 'desc'.")]
        public string SortOrder { get; set; } = "desc";
    }
}
