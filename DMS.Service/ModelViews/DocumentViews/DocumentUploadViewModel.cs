using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.DocumentViews
{
    public class DocumentUploadViewModel
    {
        public string? Id { get; set; }
        
        [Required(ErrorMessage = "You are not authorized!")]
        public string OwnerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Document name is required.")]
        [StringLength(50, ErrorMessage = "Document name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Folder selection is required.")]
        public string FolderId { get; set; } = string.Empty;

        public string? ExistingFilePath { get; set; } // For displaying old file name/path
        public string ReturnURL { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please upload a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(10 * 1024 * 1024)] // 10 MB max
        [AllowedExtensions([".pdf", ".docx", ".xlsx", ".png", ".jpg"])]
        public IFormFile? File { get; set; } // For Upload
    }
}
