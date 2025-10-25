namespace DMS.Service.ModelViews.TrashViews
{
    public class TrashViewModel
    {
        public TrashFoldersViewModel TrashFolders { get; set; } = new();
        public TrashDocumentsViewModel TrashDocuments { get; set; } = new();
    }
}
