
namespace DMS.Service.ModelViews.Shared
{
    public class SharedIndexViewModel
    {
        public List<SharedItemViewModel> SharedWithMe { get; set; } = new();
        public List<SharedItemViewModel> SharedByMe { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPagesWithMe { get; set; } = 1;
        public int TotalPagesByMe { get; set; } = 1;

        public bool HasPreviousPageWithMe => CurrentPage > 1;
        public bool HasNextPageWithMe => CurrentPage < TotalPagesWithMe;

        public bool HasPreviousPageByMe => CurrentPage > 1;
        public bool HasNextPageByMe => CurrentPage < TotalPagesByMe;

        // Search & Sort
        public string SearchTerm { get; set; } = string.Empty;
        public string SortOrder { get; set; } = "dateDesc";

    }
}
