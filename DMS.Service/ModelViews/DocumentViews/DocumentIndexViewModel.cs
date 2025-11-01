using DMS.Domain.ENums;

namespace DMS.Service.ModelViews.DocumentViews
{
    public class DocumentIndexViewModel
    {
        public string FolderId { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public string? CurrentSearch { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;
        public string? SortField { get; set; } = "AddedAt";
        public string? SortOrder { get; set; } = "desc";
        public PermissionLevel? Permission { get; set; }
        public List<DocumentListItemViewModel> DocumentList { get; set; } = new();
    }
}
