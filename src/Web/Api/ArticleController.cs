using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.ImageServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.ArticleAdminViewModels;

namespace Web.Api
{
    [Route("api/[Controller]")]
    [Authorize(Roles = "Admins")]
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public IQueryable<Article> GetAll()
        {
            return _articleRepository.GetAll();
        }

        [HttpGet("paginate")]
        public ListViewModel GetAll(ListViewModel paginate)
        {
            var query = GetAll();

            // Filtros
            if (!string.IsNullOrEmpty(paginate.Title))
            {
                query = query.Where(a => a.Title.ToLower().Contains(paginate.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.Body))
            {
                query = query.Where(a => a.Body.ToLower().Contains(paginate.Body.ToLower()));
            }

            switch (paginate.Active)
            {
                case ActiveChoices.Active:
                    query = query.Where(a => a.Active);
                    break;
                case ActiveChoices.Desactive:
                    query = query.Where(a => a.Active == false);
                    break;
                case ActiveChoices.None:
                    break;
                default:
                    throw new ArgumentNullException();
            }

            if (DateTime.TryParse(paginate.CreateAt, out var createAt))
            {
                query = query.Where(u => u.CreateAt.ToShortDateString() == createAt.ToShortDateString());
            }

            if (DateTime.TryParse(paginate.UpdateAt, out var updateAt))
            {
                query = query.Where(u => u.UpdateAt.ToShortDateString() == updateAt.ToShortDateString());
            }

            query = query
                .Include(a => a.Owner)
                .Include(a => a.DefaultTag);

            if (!string.IsNullOrEmpty(paginate.Owner))
            {
                query = query.Where(u => u.Owner.Id.ToLower().Contains(paginate.Owner));
            }

            if (paginate.DefaultTag > 0)
            {
                query = query.Where(t => t.DefaultTag.Id == paginate.DefaultTag);
            }

            if (paginate.OrderBy == "owner")
            {
                paginate.OrderBy = "Owner.UserName";
            }
            else if (paginate.OrderBy == "defaultTag")
            {
                paginate.OrderBy = "DefaultTag.Name";
            }

            paginate.Paginate(query);
            return paginate;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = _articleRepository.GetById(id);
            if (article is null)
            {
                return BadRequest();
            }

            await _articleRepository.RemoveAsync(article);
            return new NoContentResult();
        }

        [HttpPut("upload-image")]
        public async Task<IActionResult> UploadImage([FromServices] IUploadImageService uploadImageService, IFormFile file)
        {
            var imageResult = await uploadImageService.UploadAndResizeAsync(file, "blog/articles/posts", 960, 960);
            return new ObjectResult(imageResult);
        }

        [AllowAnonymous]
        [HttpPut("increase/like/{id}")]
        public async Task<IActionResult> IncreaseLike(int id)
        {
            var article = _articleRepository.GetById(id);
            if (article is null)
            {
                return BadRequest();
            }

            article.Likes++;
            await _articleRepository.UpdateAsync(article);
            return new ObjectResult(article);
        }
    }
}
