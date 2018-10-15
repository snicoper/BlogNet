using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ApplicationCore.Core.PaginatorCore
{
    public class PaginatorCore<TModel> : BasePaginatorCore where TModel : class
    {
        public PaginatorCore()
        {
            PageSize = 20;
            PageNumber = 1;
            OrderBy = "Id";
            SortOrder = SortOrder.Descending;
            JustifyContent = "justify-content-start";
            Ratio = 3;
        }

        public IQueryable<TModel> ObjectList { get; set; }

        public void Paginate(IQueryable<TModel> query)
        {
            var orderField = OrderBy.Substring(0, 1).ToUpper() + OrderBy.Substring(1);
            var sortOrder = SortOrder == SortOrder.Ascending ? "ascending" : "descending";
            var orderBy = $"{orderField} {sortOrder}";

            TotalItems = query.Count();
            TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
            ObjectList = query
                .OrderBy(orderBy)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize);
        }
    }
}
