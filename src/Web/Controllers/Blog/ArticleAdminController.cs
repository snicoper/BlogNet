using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Identity;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.EmailServices;
using ApplicationCore.Services.ImageServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Extensions;
using Web.ViewModels.ArticleAdminViewModels;
using Web.ViewModels.ArticleSubscriberViewModels;

namespace Web.Controllers.Blog
{
    [Authorize(Roles = "Admins")]
    public class ArticleAdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly ITagRepository _tagRepository;
        private readonly IUploadImageService _uploadImageService;
        private readonly IArticleRepository _articleRepository;

        private readonly string _uploadTo;
        private readonly int _maxWidth;
        private readonly int _maxHeight;

        public ArticleAdminController(
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IHostingEnvironment env,
            ITagRepository tagRepository,
            IUploadImageService uploadImageService,
            IArticleRepository articleRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _env = env;
            _tagRepository = tagRepository;
            _uploadImageService = uploadImageService;
            _articleRepository = articleRepository;

            _uploadTo = Path.Combine("blog", "articles", "headers");
            _maxWidth = 800;
            _maxHeight = 450;
        }

        public IActionResult List()
        {
            var model = new ListViewModel(_articleRepository, _tagRepository);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateEditViewModel
            {
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };
            await _initializeCreateEditViewModel(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateEditViewModel model,
            [FromServices] ISubscribeArticleRepository subscribeArticleRepository,
            [FromServices] IEmailService emailService)
        {
            await _initializeCreateEditViewModel(model);

            if (ModelState.IsValid)
            {
                if (_articleRepository.TitleExists(model.Title))
                {
                    var message = $"El titulo '{model.Title}' ya existe";
                    ModelState.AddModelError(nameof(model.Title), message);
                    return View(model);
                }

                var article = new Article
                {
                    Title = model.Title,
                    Slug = model.Slug,
                    Body = model.Body,
                    Views = model.Views,
                    Active = model.Active,
                    Likes = model.Likes,
                    CreateAt = model.CreateAt,
                    UpdateAt = model.UpdateAt,
                    OwnerId = model.OwnerId,
                    DefaultTagId = model.DefaultTagId,
                    TableOfContents = model.TableOfContents
                };

                if (model.ImageHeader != null)
                {
                    var result = await _uploadImageService.UploadAndResizeAsync(
                        model.ImageHeader,
                        _uploadTo,
                        _maxWidth,
                        _maxHeight);

                    if (result.Success)
                    {
                        var filename = result.ReturnMessages["filename"];
                        article.ImageHeader = Path.Combine(_uploadTo, filename);
                    }
                }

                await _articleRepository.CreateAsync(_tagRepository, article, model.Tags);

                // Notificar a los subscritos del nuevo articulo.
                // Solo si el artículo esta activo.
                if (model.Active)
                {
                    var subscribeList = subscribeArticleRepository
                        .GetAll()
                        .Select(s => new
                        {
                            EmailAddress = new MailAddress(s.Email),
                            s.Token
                        });

                    var siteName = _configuration["Site:SiteName"];
                    var modelEmail = new ArticleSubscribeNotifyViewModel
                    {
                        SiteName = siteName,
                        Title = article.Title,
                        CallBack = Url.Action(
                            "Details",
                            "Article",
                            new { slug = article.Slug },
                            HttpContext.Request.Scheme)
                    };

                    foreach (var subscribe in subscribeList.ToList())
                    {
                        var unsubscribe = Url.Action(
                            "UnSubscribe",
                            "ArticleSubscribe",
                            new { token = subscribe.Token },
                            HttpContext.Request.Scheme
                        );
                        modelEmail.UnsubscribeLink = unsubscribe;
                        emailService.Subject = $"Nuevo articulo en {siteName}";
                        emailService.To = new List<MailAddress> { subscribe.EmailAddress };
                        await emailService.SendEmailAsync("ArticleSubscribersNotify", modelEmail);
                    }

                    // Ping a google
                    var sitemapUrl = Url.Action("Sitemap", "Page", null, HttpContext.Request.Scheme);
                    var repGoogle = WebRequest.Create(sitemapUrl);
                    repGoogle.GetResponse();
                    this.AddMessage("success", "Articulo creado con éxito");
                }
                else
                {
                    this.AddMessage("warning", "El artículo ha sido creado, pero no esta activo");
                }

                return RedirectToAction(nameof(List));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleRepository
                .GetAll()
                .Include(a => a.TagArticles)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article is null)
            {
                return NotFound();
            }

            var model = new CreateEditViewModel
            {
                OwnerId = article.OwnerId,
                DefaultTagId = article.DefaultTagId,
                Tags = article.TagArticles.Select(t => t.TagId).ToArray(),
                Title = article.Title,
                Slug = article.Slug,
                CurrentImageHeader = article.ImageHeader,
                Body = article.Body,
                Active = article.Active,
                Views = article.Views,
                Likes = article.Likes,
                CreateAt = article.CreateAt,
                UpdateAt = DateTime.Now,
                TableOfContents = article.TableOfContents
            };

            await _initializeCreateEditViewModel(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _initializeCreateEditViewModel(model);
                return View(model);
            }

            var article = await _articleRepository
                .GetAll()
                .Include(a => a.TagArticles)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (article is null)
            {
                return NotFound();
            }

            var oldImageHeader = article.ImageHeader;

            article.Title = model.Title;
            article.Slug = model.Slug;
            article.Body = model.Body;
            article.Views = model.Views;
            article.Active = model.Active;
            article.Likes = model.Likes;
            article.CreateAt = model.CreateAt;
            article.UpdateAt = model.UpdateAt;
            article.OwnerId = model.OwnerId;
            article.DefaultTagId = model.DefaultTagId;
            article.TableOfContents = model.TableOfContents;

            if (model.ImageHeader != null && oldImageHeader != model.ImageHeader.FileName)
            {
                var result = await _uploadImageService.UploadAndResizeAsync(
                    model.ImageHeader,
                    _uploadTo,
                    _maxWidth,
                    _maxHeight);

                if (result.Success)
                {
                    var filename = result.ReturnMessages["filename"];
                    article.ImageHeader = Path.Combine(_uploadTo, filename);

                    if (oldImageHeader != null)
                    {
                        var oldImageHeaderPath = Path.Combine(
                            _env.WebRootPath,
                            _configuration["Images:Path"].Trim('/').Replace('/', Path.DirectorySeparatorChar),
                            oldImageHeader);

                        if (System.IO.File.Exists(oldImageHeaderPath))
                        {
                            System.IO.File.Delete(oldImageHeaderPath);
                        }
                    }
                }
            }

            await _articleRepository.UpdateAsync(_tagRepository, article, model.Tags);
            this.AddMessage("success", "Artículo editado con éxito");
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var article = _articleRepository.GetById(id);
            if (article is null)
            {
                return NotFound();
            }

            await _articleRepository.RemoveAsync(article);
            this.AddMessage("success", "Artículo eliminado con éxito");
            return RedirectToAction(nameof(List));
        }

        private async Task _initializeCreateEditViewModel(CreateEditViewModel model)
        {
            var tags = _tagRepository.GetAll();

            foreach (var tag in tags)
            {
                model.TagList.Add(new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.Name
                });
            }

            if (!string.IsNullOrEmpty(model.OwnerId) && string.IsNullOrEmpty(model.OwnerUserName))
            {
                var owner = await _userManager.FindByIdAsync(model.OwnerId);
                if (owner != null)
                {
                    model.OwnerUserName = owner.UserName;
                }
            }
            else if (string.IsNullOrEmpty(model.OwnerId))
            {
                var owner = await _userManager.FindByNameAsync(User.Identity.Name);
                model.OwnerId = owner.Id;
                model.OwnerUserName = owner.UserName;
            }
        }
    }
}
