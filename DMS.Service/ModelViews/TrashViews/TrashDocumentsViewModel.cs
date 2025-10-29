namespace DMS.Service.ModelViews.TrashViews
{
    public class TrashDocumentsViewModel
    {
        public PagedResult<TrashedDocumentViewModel> TrashedDocuments { get; set; } = new();
    }
}
