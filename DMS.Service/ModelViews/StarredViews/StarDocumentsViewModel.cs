using DMS.Service.ModelViews.TrashViews;

namespace DMS.Service.ModelViews.StarredViews
{
    public class StarDocumentsViewModel
    {
        public PagedResult<StarredDocumentViewModel> StarredDocument { get; set; } = new();
    }
}
