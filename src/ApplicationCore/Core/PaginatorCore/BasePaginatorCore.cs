namespace ApplicationCore.Core.PaginatorCore
{
    public enum SortOrder
    {
        Ascending = 1,
        Descending = 2
    }

    public class BasePaginatorCore
    {
        public SortOrder SortOrder { get; set; }
        public string OrderBy { get; set; }
        public int Ratio { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public string JustifyContent { get; set; }
    }
}
