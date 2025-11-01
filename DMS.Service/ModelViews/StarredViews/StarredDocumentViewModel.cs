namespace DMS.Service.ModelViews.StarredViews
{
    public class StarredDocumentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public bool IsStarred { get; set; }
        public DateTime AddedAt { get; set; }
        public int Size { get; set; }
        public string FolderId { get; set; } = string.Empty;
        public string? FolderName { get; set; }
    }
}
