using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ApplicationCore.Core.RssCore;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Identity;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.EmailServices;
using Markdig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Extensions;
using Web.ViewModels.ArticleViewModels;

namespace Web.Controllers.Blog
{
    public class ArticleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IArticleRepository _articleRepository;

        public ArticleController(
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IArticleRepository articleRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _articleRepository = articleRepository;
        }

        public IActionResult List(int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }

            var items = _articleRepository.GetListArticles();

            var model = new ListViewModel
            {
                PageNumber = page
            };
            model.Paginate(items);
            return View(model);
        }

        public IActionResult ListByTagName(string tagSlug, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }

            var items = _articleRepository
                .GetListArticles()
                .Where(a => a.DefaultTag.Slug == tagSlug);

            var model = new ListViewModel
            {
                PageNumber = page
            };
            model.Paginate(items);
            return View(model);
        }

        public IActionResult Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var article = _articleRepository.GetBySlug(slug);
            if (article is null)
            {
                return NotFound();
            }

            // Si no esta activo, solo lo puede ver un administrador
            if (!article.Active && !User.IsInRole("Admins"))
            {
                return NotFound();
            }

            var model = new DetailsViewModel
            {
                Article = article,
                PreviousArticle = _articleRepository.GetPreviousArticle(article),
                NextArticle = _articleRepository.GetNextArticle(article)
            };

            if (User.IsInRole("Admins") is false)
            {
                article.Views = article.Views + 1;
                _articleRepository.UpdateAsync(article);
            }

            return View(model);
        }

        public IActionResult Search(string q, int page = 1)
        {
            var items = new List<Article>().AsQueryable();
            var model = new SearchResultViewModel
            {
                q = q,
                PageNumber = 1
            };

            if (string.IsNullOrEmpty(q))
            {
                model.Paginate(items);
                return View(model);
            }

            items = _articleRepository
                .GetListArticles()
                .Where(t => t.Title.ToLower().Contains(q.ToLower()) || t.Body.ToLower().Contains(q.ToLower()));

            model.Paginate(items);
            return View(model);
        }

        public IActionResult Rss()
        {
            var rss = new RssSyndicationCore
            {
                Title = _configuration["Site:SiteName"],
                Description = $"Últimos artículos en {_configuration["Site:SiteName"]}",
                Link = _configuration["Site:SiteUrl"]
            };

            var articles = _articleRepository.GetRssArticles(10);

            foreach (var article in articles)
            {
                var scheme = Url.ActionContext.HttpContext.Request.Scheme;
                rss.RssItems.Add(new RssArticle
                {
                    AuthorName = article.Owner.UserName,
                    AuthorEmail = article.Owner.Email,
                    Title = article.Title,
                    Body = Markdown.ToHtml(article.Body),
                    CreateAt = article.CreateAt,
                    Link = Url.Action("Details", "Article", new { article.Slug }, scheme),
                    Permalink = Url.Action("Details", "Article", new { article.Slug }, scheme)
                });
            }

            return Content(rss.Serialize().ToString(), "text/xml");
        }

        public async Task<IActionResult> Recommend(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var article = _articleRepository.GetBySlug(slug);
            if (article is null)
            {
                return NotFound();
            }

            var email = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                email = user.Email;
            }

            var model = new RecommendViewModel
            {
                Slug = article.Slug,
                Title = article.Title,
                From = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Recommend(
            [FromServices] IEmailService emailService,
            string slug,
            RecommendViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var article = _articleRepository.GetBySlug(model.Slug);
            if (article is null)
            {
                return NotFound();
            }

            var siteName = _configuration["Site:SiteName"];
            var emailModel = new EmailRecommendViewModel
            {
                SiteName = siteName,
                Title = article.Title,
                CallBack = Url.Action(nameof(Details), "Article", new { article.Slug }, HttpContext.Request.Scheme),
                From = model.From
            };

            emailService.Subject = $"Recomendación de articulo en {siteName}";
            emailService.From = new MailAddress(model.From);
            emailService.To = new List<MailAddress> { new MailAddress(model.To) };
            await emailService.SendEmailAsync("ArticleRecommend", emailModel);
            this.AddMessage("success", "Recomendación enviada con éxito");
            return RedirectToAction(nameof(Details), new { model.Slug });
        }
    }
}
