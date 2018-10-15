using System.IO;
using ApplicationCore.Core.SitemapCore;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.ViewModels.PageViewModels;

namespace Web.Controllers
{
    public class PageController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IArticleRepository _articleRepository;

        public PageController(IConfiguration configuration, IArticleRepository articleRepository)
        {
            _configuration = configuration;
            _articleRepository = articleRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult CookieConsent()
        {
            var model = new CookieConsentViewModel
            {
                SiteName = _configuration["Site:SiteName"]
            };
            return View(model);
        }

        public IActionResult Sitemap()
        {
            var articles = _articleRepository.GetAllActive();
            var siteMap = new SitemapCore();

            foreach (var article in articles)
            {
                var url = Url.Action("Details", "Article", new { slug = article.Slug }, HttpContext.Request.Scheme);
                siteMap.AddUrl(url, article.UpdateAt);
            }

            return Content(siteMap.Serialize().ToString(), "text/xml");
        }

        public string Robots([FromServices] IHostingEnvironment env)
        {
            var path = Path.Combine(env.WebRootPath, "static", "robots.txt");
            return System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path) : string.Empty;
        }
    }
}
