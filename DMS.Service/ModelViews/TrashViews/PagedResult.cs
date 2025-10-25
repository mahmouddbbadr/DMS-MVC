namespace DMS.Service.ModelViews.TrashViews
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public string? CurrentSearch { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;
        public string? SortField { get; set; } = "DeletedAt";
        public string? SortOrder { get; set; } = "desc";
    }

}
