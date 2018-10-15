using ApplicationCore.Data.Entities.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.ViewModels;

namespace Web.ViewComponents
{
    public class DisqusCommentsViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;

        public DisqusCommentsViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IViewComponentResult Invoke(Article article)
        {
            var urlArticle = Url.Action(
                "Details",
                "Article",
                new { article.Slug },
                HttpContext.Request.Scheme);

            var model = new DisqusCommentsViewModel
            {
                ShortName = _configuration["Disqus:ShortName"],
                Url = urlArticle,
                Identifier = article.Id
            };
            return View(model);
        }
    }
}
