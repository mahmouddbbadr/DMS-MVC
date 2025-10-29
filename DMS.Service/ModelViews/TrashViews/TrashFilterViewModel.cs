using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DMS.Service.ModelViews.TrashViews
{
    public class TrashFilterViewModel
    {
        [ValidateNever]
        public string UserId { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters.")]
        [Display(Name = "Search")]
        public string? SearchTerm { get; set; }

        [RegularExpression("^(Name|FolderName|DeletedAt|Size|ItemCount)?$", ErrorMessage = "Invalid sort field.")]
        public string? SortField { get; set; } = "DeletedAt";

        [RegularExpression("^(asc|desc)?$", ErrorMessage = "Sort order must be either 'asc' or 'desc'.")]
        public string? SortOrder { get; set; } = "desc";

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1.")]
        public int PageNum { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 5;
    }

}
