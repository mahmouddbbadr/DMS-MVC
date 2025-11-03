using DMS.Service.ModelViews.DocumentViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.Folder
{
    public class FolderIndexViewModel
    {
        //public string? ParentFolderId { get; set; }
        //public string? ParentFolderName { get; set; }
        public int CurrentPage { get; set; }
        public string? CurrentSearch { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
        public List<FolderListItemViewModel> FolderList { get; set; } = new();
    }
}
