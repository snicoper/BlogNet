using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.TagAdminViewModels;

namespace Web.Api
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admins")]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpPost]
        public IQueryable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        [HttpGet("paginate")]
        public ListViewModel GetAll(ListViewModel paginate)
        {
            var query = GetAll()
                .Include(a => a.TagArticles)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrEmpty(paginate.Name))
            {
                query = query.Where(t => t.Name.ToLower().Contains(paginate.Name.ToLower()));
            }

            paginate.OrderBy = paginate.OrderBy == "tagArticles" ? "TagArticles.Count" : paginate.OrderBy;
            paginate.OrderBy = paginate.OrderBy == "articles" ? "Articles.Count" : paginate.OrderBy;
            query = query.Include(t => t.Articles);
            paginate.Paginate(query);

            return paginate;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag is null)
            {
                return NotFound();
            }

            await _tagRepository.RemoveAsync(tag);
            return NoContent();
        }

        [HttpGet("search/name")]
        public IActionResult FilterByName(string term)
        {
            var tags = GetAll()
                .Where(u => u.Name.ToLower().Contains(term.ToLower()))
                .Select(u => new
                {
                    u.Id,
                    Text = u.Name
                });
            return new ObjectResult(tags);
        }
    }
}
