namespace DMS.Service.ModelViews.TrashViews
{
    public class TrashedFolderViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? ItemCount { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? ParentFolderName { get; set; }
    }
}
