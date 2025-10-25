using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.DocumentViews
{
    public class DocumentUploadViewModel
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FolderId { get; set; }
        public string? ExistingFilePath { get; set; } // show old file name/path
        public IFormFile? File { get; set; } // for Upload
    }
}
