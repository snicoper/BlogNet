using ApplicationCore.Core.PaginatorCore;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.ViewComponents
{
    public class PaginatorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(BasePaginatorCore paginator)
        {
            var numMaxPages = paginator.Ratio * 2 + 1;
            var model = new PaginatorViewModel
            {
                BasePaginatorCore = paginator,
                QueryString = HttpContext.Request.QueryString,
                FirstPage = 1,
                LastPage = paginator.TotalPages > numMaxPages ? numMaxPages : paginator.TotalPages
            };

            if (paginator.PageNumber > paginator.Ratio + 1)
            {
                model.FirstPage = paginator.PageNumber - paginator.Ratio;
                if (paginator.TotalPages > paginator.PageNumber + paginator.Ratio)
                {
                    model.LastPage = paginator.PageNumber + paginator.Ratio;
                }
                else
                {
                    model.LastPage = paginator.TotalPages;
                }
            }

            return View(model);
        }
    }
}
