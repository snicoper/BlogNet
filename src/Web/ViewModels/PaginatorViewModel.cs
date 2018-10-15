using ApplicationCore.Core.PaginatorCore;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels
{
    public class PaginatorViewModel
    {
        public BasePaginatorCore BasePaginatorCore { get; set; }
        public QueryString QueryString { get; set; }
        public int FirstPage { get; set; }
        public int LastPage { get; set; }
    }
}
