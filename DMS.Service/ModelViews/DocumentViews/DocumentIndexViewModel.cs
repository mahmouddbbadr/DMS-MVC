namespace DMS.Service.ModelViews.DocumentViews
{
    public class DocumentIndexViewModel
    {
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public int CurrentPage { get; set; }
        public string? CurrentSearch { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
        public List<DocumentListItemViewModel> DocumentList { get; set; } = new();
    }
}
